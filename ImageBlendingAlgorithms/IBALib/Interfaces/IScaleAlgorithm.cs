using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBALib.Interfaces
{
    public interface IScaleAlgorithm : IAlgorithm
    {
        Colour[,] Scale<T>(IMatrix<T> src, int x, int y);
    }
}
