using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Processing.ImageProcessing.Interfaces
{
    public interface IImageProcessorCommand
    {
        IEnumerable<ImageWrapper<T>> Perform<T>(IEnumerable<ImageWrapper<T>> images);
    }
}
