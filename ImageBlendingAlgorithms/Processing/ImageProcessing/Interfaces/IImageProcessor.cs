using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Processing.ImageProcessing.Interfaces
{
    public interface IImageProcessor<T>
    {
        void Process();
        void AddCommand(IImageProcessorCommand command);
        void RemoveCommand(IImageProcessorCommand command);
        void AddImage(ImageWrapper<T> image);
        void AddImage(IEnumerable<ImageWrapper<T>> images);
        void RemoveImage(ImageWrapper<T> image);

        ImageWrapper<T> Result { get; }
    }
}
