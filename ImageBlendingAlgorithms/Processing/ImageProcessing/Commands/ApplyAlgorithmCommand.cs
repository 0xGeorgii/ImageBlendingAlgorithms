using IBALib.Interfaces;
using IBALib.Types;
using Processing.ImageProcessing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.ImageProcessing.Commands
{
    public class ApplyAlgorithmCommand : IImageProcessorCommand
    {
        private readonly IBlendAlgorithm _algorithm;
        private readonly Type _imageType;

        public ApplyAlgorithmCommand(IBlendAlgorithm algorithm, Type imageType)
        {
            _algorithm = algorithm;
            _imageType = imageType;
        }

        public IEnumerable<ImageWrapper<T>> Perform<T>(IEnumerable<ImageWrapper<T>> images)
        {
            var h = images.ElementAt(0).Height;
            var w = images.ElementAt(0).Width;

            var genericType = _imageType.MakeGenericType(typeof(T));
            dynamic res = null;
            try
            {
                res = Activator.CreateInstance(genericType, new object[] { w, h });

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        var pixels = images.Select(img =>
                        {
                            dynamic pixel = img[i, j];
                            return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
                        });
                        res[i, j] = (T)Activator.CreateInstance(typeof(T), new object[] { _algorithm.Calculate(pixels).Vector4 });
                    }
                }
                return new List<ImageWrapper<T>>(1) { new ImageWrapper<T>(res) };
            }
            catch(Exception ex)
            {
                if (res != null && res is IDisposable) (res as IDisposable).Dispose();
                return images;
            }
        }
   }
}
