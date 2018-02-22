using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using IBALib;
using IBALib.Interfaces;
using IBALib.Types;
using Processing;
using Processing.ImageProcessing;
using Processing.ImageProcessing.Commands;
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
        private bool __isStared;
        private bool _isStarted
        {
            get
            {
                return __isStared;
            }
            set
            {
                __isStared = value;
                SwitchUIEnable();
            }
        }
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
                _algorithmsCheckboxes.ForEach(cb => cb.IsEnabled = !(sender as CheckBox).IsChecked.Value);
            };
            var outputTB = this.FindControl<TextBlock>("output");
            Log.RegisterCallback((message) =>
            {
                Dispatcher.UIThread.InvokeAsync(() => outputTB.Text += message);
            });
            Log.Debug("=== Desktop UI has been initialized ===");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoaderPortableXaml.Load(this);
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
            if (_isStarted)
            {
                var processTasks = new Task[cyclesCount];
                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;
                for (int i = 0; i < cyclesCount; i++)
                {
                    processTasks[i] = Process(heigth, width, imagesCount);
                }
                await Task.WhenAll(processTasks);
            }
            else if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
                GC.Collect();
            }
        }

        private void SwitchUIEnable()
        {
            _heightTB.IsEnabled = 
                _widthTB.IsEnabled =
                _roundTripCb.IsEnabled =
                _cyclesCountTB.IsEnabled =
                !_isStarted;
            _algorithmsCheckboxes.ForEach(c => c.IsEnabled = !_isStarted);
        }

        private async Task Process(string heigth, string width, int imagesCount)
        {
            var tasks = new Task<Image<Rgba32>>[imagesCount];
            try
            {
                for (int i = 0; i < imagesCount; i++)
                {
                    tasks[i] = SrcLoader.DownloadImageAsync(uint.Parse(width), uint.Parse(heigth), _cancellationToken);
                }
                await Task.WhenAll(tasks);
                var ts = TaskScheduler.FromCurrentSynchronizationContext();
                var checkedCb = _algorithmsCheckboxes.Where(c => _roundTripCb.IsChecked.Value || c.IsChecked.Value).ToList();
                var tasksForGenerating = new Task[checkedCb.Count];
                for (int i = 0; i < checkedCb.Count; i++)
                {
                    int n = i;
                    tasksForGenerating[i] = new Task(() => GenerateImage(tasks, _algorithms[checkedCb.ElementAt(n).Name]), _cancellationToken);
                    tasksForGenerating[i].Start();
                }
                await Task.WhenAll(tasksForGenerating);
                if (_isStarted) StartButtonHandle();
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    //TODO: log it and don't throw
                }
                else
                    throw;
            }
            finally
            {
                tasks.ToList().ForEach(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion) x.Result.Dispose();
                    x = null;
                });
            }
        }

        private void GenerateImage(Task<Image<Rgba32>>[] tasks, IBlendAlgorithm algorithm)
        {
            var proc = new ImageProcessor<Rgba32>();
            proc.AddCommand(new ApplyAlgorithmCommand(algorithm, typeof(Image<>)));
            proc.AddCommand(new ScaleImageCommand(AlgorithmFactory.Instance.ScalingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.NearestNeighbor], 400, 400, typeof(Image<>)));
            proc.AddImage(tasks.Select(t => new ImageWrapper<Rgba32>(t.Result)));
            proc.Process();
            using (var fs = File.Create($"./{Guid.NewGuid().ToString()}.jpg"))
            {
                ((proc.Result as dynamic).GetSource as Image<Rgba32>).SaveAsJpeg(fs);
            }
            
            /*
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
            */
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
