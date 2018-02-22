using IBALib.Interfaces;
using IBALib.Types;
using Processing.ImageProcessing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.ImageProcessing.Commands
{
    public class ScaleImageCommand : IImageProcessorCommand
    {
        private readonly IScaleAlgorithm _algorithm;
        private readonly Type _imageType;
        private int _x, _y;

        public ScaleImageCommand(IScaleAlgorithm algorithm, int x, int y, Type imageType)
        {
            _algorithm = algorithm;
            _x = x;
            _y = y;
            _imageType = imageType;
        }

        public IEnumerable<ImageWrapper<T>> Perform<T>(IEnumerable<ImageWrapper<T>> images)
        {
            var isNeedScale = false;
            var lst = new List<ImageWrapper<T>>(images);
            var nx = lst[0].Height;
            var ny = lst[0].Width;
            foreach (var image in images)
            {
                if(image.Height != nx || image.Width != ny)
                {
                    isNeedScale = true;
                    break;
                }
            }
            if (!isNeedScale) return images;

            var genericType = _imageType.MakeGenericType(typeof(T));
            dynamic res = null;
            for (int i = 0; i < lst.Count(); i++)
            {
                try
                {
                    res = Activator.CreateInstance(genericType, new object[] { _x, _y });
                    var pixels = _algorithm.Scale(new ImageWrapper<T>(lst[i]), _x, _y);
                    for (int k = 0; i < _x; k++)
                    {
                        for (int n = 0; n < _y; n++)
                        {
                            res[k, n] = (T)Activator.CreateInstance(typeof(T), new object[] { (pixels[k, n].Vector4) });
                        }
                    }
                    lst[i] = new ImageWrapper<T>(res);
                }
                catch(Exception ex)
                {
                    if (res != null && res is IDisposable) (res as IDisposable).Dispose();
                }
            }
            return lst;
        }
    }
}
