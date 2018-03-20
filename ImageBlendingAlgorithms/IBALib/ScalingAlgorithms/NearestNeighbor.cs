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

        public virtual string GetVerboseName() => "Nearest Neighbor";

        public Colour[,] Scale<T>(IMatrix<T> src, int x, int y)
        {
            if (src.Width == x && src.Height == y) return null;
            var res = new Colour[x,y];
            var xRatio = CalcXRatio(src.Width, x);
            var yRatio = CalcYRatio(src.Height, y);
            double px, py;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    px = Math.Floor(i * xRatio);
                    py = Math.Floor(j * yRatio);
                    res[i, j].FillFrom(Colour.FromObject(src[(int)px, (int)py]));
                    res[i, j].UpdateRawData();
                }
            }
            return res;
        }

        protected virtual void CheckIfAlgorithmSuitable(int originX, int x, int originY, int y)
        {
            if (originX > x || originY > y) throw new ArgumentException("x and y should't be not the less than source image dimentions");
        }
        protected virtual double CalcXRatio(int originX, int x) => (double)originX / x;
        protected virtual double CalcYRatio(int originY, int y) => (double)originY / y;
    }
}
