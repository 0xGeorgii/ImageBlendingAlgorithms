using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IBALib.Interfaces;
using IBALib.Types;
using SixLabors.ImageSharp;
using SourceProvider.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopUI
{
    public class MainWindow : Window
    {
        private bool _isStarted = false;
        private Grid _grid;
        private Button _button;
        private TextBox _heightTB;
        private TextBox _widthTB;
        private TextBox _imagesCountTB;
        private CheckBox _roundTripCb;
        private List<CheckBox> _algorithmsCheckboxes;
        private static ConcurrentDictionary<string, IBlendAlgorithm> _algorithms = new ConcurrentDictionary<string, IBlendAlgorithm>();

        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();
            _grid = this.FindControl<Grid>("grid");
            _algorithmsCheckboxes = new List<CheckBox>();
            BuildComponents();
            _button = this.FindControl<Button>("startBtn");
            _button.Click += delegate { StartButtonHandle(); };
            _widthTB = this.FindControl<TextBox>("widthTB");
            _heightTB = this.FindControl<TextBox>("heightTB");
            _imagesCountTB = this.FindControl<TextBox>("imagesCountTB");
            _roundTripCb = this.FindControl<CheckBox>("rndTripCb");
            _roundTripCb.Click += (sender, args) =>
            {
                _algorithmsCheckboxes.ForEach(cb => cb.IsEnabled = !(sender as CheckBox).IsChecked);
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BuildComponents()
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                int row = 0;
                foreach (var type in assembly.GetTypes())
                {
                    if(type.GetCustomAttributes(typeof(ImageBlendingAlgorithmAttribute), false).Length > 0)
                    {
                        var cb = new CheckBox
                        {
                            Content = type.Name,
                            Height = 25,
                            Name = type.Name
                        };
                        _algorithms.TryAdd(type.Name, Activator.CreateInstance(type) as IBlendAlgorithm);
                        _algorithmsCheckboxes.Add(cb);
                        _grid.Children.Add(cb);
                        if (row > 3)
                            _grid.RowDefinitions.Add(new RowDefinition());
                        Grid.SetColumn(cb, 3);
                        Grid.SetRow(cb, row++);
                    }
                }
            }
        }

        private async void StartButtonHandle()
        {
            var heigth = _heightTB.Text;
            var width = _widthTB.Text;
            var imagesCount = int.Parse(_imagesCountTB.Text ?? "0");
            _isStarted = !_isStarted;
            _button.Content = _isStarted ? "Stop" : "Start";
            _heightTB.IsEnabled = _widthTB.IsEnabled = _roundTripCb.IsEnabled = !_isStarted;
            if (_isStarted)
            {
                var tasks = new Task<Image<Rgba32>>[imagesCount];
                for (int i = 0; i < imagesCount; i++)
                {
                    tasks[i] = SrcLoader.DownloadImageAsync(uint.Parse(width), uint.Parse(heigth));
                }
                await Task.WhenAll(tasks);
                try
                {
                    if (_roundTripCb.IsChecked)
                    {
                    }
                    else
                    {
                        var ts = TaskScheduler.FromCurrentSynchronizationContext();
                        await Task.Factory.StartNew(() => GenerateImage(tasks))
                            .ContinueWith((t)=> { if (_isStarted) StartButtonHandle(); }, ts);
                    }
                }
                catch
                {
                    tasks.ToList().ForEach(x =>
                    {
                        if (x.Status == TaskStatus.RanToCompletion) x.Result.Dispose();
                    });
                    throw;
                }
            }
        }

        private void GenerateImage(Task<Image<Rgba32>>[] tasks)
        {
            _algorithmsCheckboxes.ForEach(cb =>
            {
                if (cb.IsChecked)
                {
                    var h = tasks[0].Result.Height;
                    var w = tasks[0].Result.Width;
                    var alg = _algorithms[cb.Name];
                    using (var res = new SixLabors.ImageSharp.Image<Rgba32>(w, h))
                    using (var fs = File.Create($"./{Guid.NewGuid().ToString()}.jpg"))
                    {
                        for (int i = 0; i < h; i++)
                        {
                            for (int j = 0; j < w; j++)
                            {
                                var pixels = tasks.Select(t =>
                                {
                                    var pixel = t.Result[j, i];
                                    return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
                                });
                                res[j, i] = new Rgba32((alg.Calculate(pixels).Vector3));
                            }
                        }
                        res.SaveAsJpeg(fs);
                    }
                }
            });
        }
    }
}
