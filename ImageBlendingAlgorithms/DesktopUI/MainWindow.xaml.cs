using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IBALib.Types;
using SourceProvider.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
                        var cb = new CheckBox { Content = type.Name, Height = 25, Name = type.Name };
                        cb.Click += (sender, args) => 
                        {
                        };
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
                var tasks = new Task<string>[imagesCount];
                for (int i = 0; i < imagesCount; i++)
                {
                    tasks[i] = SrcLoader.DownloadAndSaveSrc(uint.Parse(width), uint.Parse(heigth));
                }
                await Task.WhenAll(tasks);
                StartButtonHandle();
            }
        }
    }
}
