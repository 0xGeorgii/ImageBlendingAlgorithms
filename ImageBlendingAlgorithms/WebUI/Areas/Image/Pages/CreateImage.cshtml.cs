using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBALib;
using IBALib.Interfaces;
using IBALib.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Processing.ImageProcessing;
using Processing.ImageProcessing.Commands;
using SixLabors.ImageSharp;
using SourceProvider.Network;
using WebUI.Utils;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebUI.Areas.Image
{
    public class AlgorithmViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public IBlendAlgorithm Algorithm {get; set; }        
    }
    public class CreateImageModel : PageModel
    {        
        [BindProperty, Range(10, 2160)]
        public int Height { get; set; } = 1080;
        [BindProperty]
        public int Width { get; set; } = 1920;
        [BindProperty]
        public int ResultHeight { get; set; } = 1080;
        [BindProperty]
        public int ResultWidth { get; set; } = 1920;
        [BindProperty, Required]
        public int ImagesCount { get; set; } = 3;
        [BindProperty, Required]
        public int CyclesCount { get; set; } = 1;
        [BindProperty]
        public bool IsRoundTrip { get; set; }       
        [BindProperty]
        public List<AlgorithmViewModel> AlgorithmsVMs { get; set; }
        private static List<AlgorithmViewModel> _algorithmsVMs;
        private readonly IHostingEnvironment _hostingEnvironment;
        static CreateImageModel()
        {
            _algorithmsVMs = new List<AlgorithmViewModel>(10);
            LoadAlgorithms();                       
        }

        public CreateImageModel(IHostingEnvironment  hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private static void LoadAlgorithms()
        {
            if(_algorithmsVMs.Any())
            {
                return;
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var algorithms = assembly
                    .GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(ImageBlendingAlgorithmAttribute), false).Length > 0).ToList();
                if (algorithms.Any())
                {
                    foreach(var a in algorithms)
                    {
                        var alg = Activator.CreateInstance(a) as IBlendAlgorithm;
                        _algorithmsVMs.Add(new AlgorithmViewModel 
                        {
                            Name = alg.GetVerboseName(),
                            IsSelected = false,
                            Algorithm = alg
                        });
                    }
                }                
            }
        }
        
        public IActionResult OnGet()
        {
            if(!_algorithmsVMs.Any()){
                LoadAlgorithms();
            }
            AlgorithmsVMs = _algorithmsVMs;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {                
                return Page();
            }

            if(!AlgorithmsVMs.Any())
            {
                LoadAlgorithms();
                AlgorithmsVMs = _algorithmsVMs;
            }
            
            var selectedAlgName = AlgorithmsVMs.First(a => a.IsSelected).Name;
            var algorithm = _algorithmsVMs.First(a => a.Name.EqualsIgnoreSpaces(selectedAlgName)).Algorithm;

            var uHeigth = (uint) Height;
            var uWidth = (uint) Width;

            var tasks = new Task<Image<Rgba32>>[ImagesCount];
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            for (int i = 0; i < ImagesCount; i++)
            {
                tasks[i] = SrcLoader.DownloadImageAsync(uWidth, uHeigth, cancellationToken);
            }

            await Task.WhenAll(tasks);

            var proc = new ImageProcessor<Rgba32>();
            proc.AddCommand(new ApplyAlgorithmCommand(algorithm, typeof(Image<>)));
            var resHeight = (uint) ResultHeight;
            var resWidth = (uint) ResultWidth;

            if(resHeight != uHeigth || resWidth != uWidth)
            {
                proc.AddCommand(new ScaleImageCommand(AlgorithmFactory.Instance.ScalingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.NearestNeighborDownscale], (int)resWidth, (int)resHeight, typeof(Image<>)));
            }
                            
            proc.AddImage(tasks.Select(t => new ImageWrapper<Rgba32>(t.Result)));
            proc.Process();
            var imgName = $"images/tmp/{Guid.NewGuid().ToString()}.jpg";
            var imgPath = Path.Combine(_hostingEnvironment.WebRootPath, imgName);
            using (var fs = System.IO.File.Create(imgPath))
            {
                ((proc.Result as dynamic).GetSource as Image<Rgba32>).SaveAsJpeg(fs);
            }

            ViewData["ResImagePath"] = $"/{imgName}";

            return Page();
        }

    }
}