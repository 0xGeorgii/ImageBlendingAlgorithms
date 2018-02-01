using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBALib.ScalingAlgorithms
{
    internal class NearestNeighbor : IScaleAlgorithm
    {
        public string GetName() => GetType().Name;

        public string GetVerboseName() => "Nearest Neighbor";

        public Color[,] Scale<T>(IMatrix<T> src, int x, int y)
        {
            if (src.Width == x && src.Height == y) return null;
            if (src.Width > x || src.Height > y) throw new ArgumentException("x and y should't be not the less than source image dimentions");
            var res = new Color[x,y];
            var xRatio = (double)src.Height / x;
            var yRatio = (double)src.Width / y;
            double px, py;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    px = Math.Floor(i * xRatio);
                    py = Math.Floor(j * yRatio);
                    res[i, j].FillFrom(Color.FromObject(src[(int)px, (int)py]));
                    res[i, j].UpdateRawData();
                }
            }
            return res;
        }
    }
}
