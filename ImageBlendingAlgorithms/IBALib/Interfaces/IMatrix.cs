using System;
using System.Collections.Generic;
using System.Text;

namespace IBALib.Interfaces
{
    public interface IMatrix<T>
    {
        T this[int x, int y] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}
