using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IBALib;
using IBALib.Interfaces;
using IBALib.Types;
using SixLabors.ImageSharp;
using SourceProvider.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        private TextBox _cyclesCountTB;
        private CheckBox _roundTripCb;
        private List<CheckBox> _algorithmsCheckboxes;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
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
            _cyclesCountTB = this.FindControl<TextBox>("cyclesCountTB");
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
            var cyclesCount = int.Parse(_cyclesCountTB.Text ?? "0");
            _isStarted = !_isStarted;
            _button.Content = _isStarted ? "Stop" : "Start";
            _heightTB.IsEnabled = _widthTB.IsEnabled = _roundTripCb.IsEnabled = !_isStarted;
            if (_isStarted)
            {
                var processTasks = new Task[cyclesCount];
                for (int i = 0; i < cyclesCount; i++)
                {
                    processTasks[i] = Process(heigth, width, imagesCount);
                }
                await Task.WhenAll(processTasks);
            }
        }

        private async Task Process(string heigth, string width, int imagesCount)
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
                    _cancellationTokenSource = new CancellationTokenSource();
                    _cancellationToken = _cancellationTokenSource.Token;
                    var ts = TaskScheduler.FromCurrentSynchronizationContext();
                    var checkedCb = _algorithmsCheckboxes.Where(c => _roundTripCb.IsChecked || c.IsChecked).ToList();                    
                    var tasksForGenerating = new Task[checkedCb.Count];
                    for (int i = 0; i < checkedCb.Count; i++)
                    {
                        int n = i;
                        tasksForGenerating[i] = new Task(() => GenerateImage(tasks, _algorithms[checkedCb.ElementAt(n).Name]));
                        tasksForGenerating[i].Start();
                    }
                    await Task.WhenAll(tasksForGenerating);
                    if (_isStarted) StartButtonHandle();
                }
                catch(Exception ex)
                {
                    tasks.ToList().ForEach(x =>
                    {
                        if (x.Status == TaskStatus.RanToCompletion) x.Result.Dispose();
                    });
                    if(ex is TaskCanceledException)
                    {
                        //TODO: log it and don't throw
                    }
                    else
                        throw;
                }
            }
            else if(_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
        }

        private void GenerateImage(Task<Image<Rgba32>>[] tasks, IBlendAlgorithm algorithm)
        {
            var h = tasks[0].Result.Height;
            var w = tasks[0].Result.Width;
            var images = tasks.Select(t => t.Result);

            using (var res = new Image<Rgba32>(w, h))
            using (var fs = File.Create($"./{Guid.NewGuid().ToString()}.jpg"))
            {
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        var pixels = images.Select(img =>
                        {
                            var pixel = img[i, j];
                            return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
                        });
                        res[i, j] = new Rgba32(algorithm.Calculate(pixels).Vector4);
                    }
                }
                res.SaveAsJpeg(fs);
            }
        }
        
        private void ScaleImage(Image<Rgba32> image, int x, int y)
        {
            if (image.Width == x || image.Height == y) return;
            var salg = AlgorithmFactory.Instance.ScalingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.NearestNeighbor];
            var img = salg.Scale(new ImageWrapper<Rgba32>(image), x, y);
            using (var res = new Image<Rgba32>(x, y))
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        res[i, j] = new Rgba32(img[i, j].Vector4);
                    }
                }
                var fs1 = File.Create($"./{Guid.NewGuid().ToString()}.jpg");
                res.SaveAsJpeg(fs1);
            }
        }
    }
}
