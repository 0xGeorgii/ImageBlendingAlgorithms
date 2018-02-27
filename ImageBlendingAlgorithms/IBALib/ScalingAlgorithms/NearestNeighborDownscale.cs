using System;
using System.Collections.Generic;
using System.Text;

namespace IBALib.ScalingAlgorithms
{
    internal class NearestNeighborDownscale : NearestNeighbor
    {
        public NearestNeighborDownscale()
        {
        }

        public override string GetVerboseName() => "Nearest Neighbor Downscale";

        protected override double CalcXRatio(int originX, int x) => (double) x / originX;

        protected override double CalcYRatio(int originY, int y) => (double)y / originY;

        protected override void CheckIfAlgorithmSuitable(int originX, int x, int originY, int y)
        {
            if (originX < x || originY < y) throw new ArgumentException("x and y should't be not bigger than source image dimentions");
        }
    }
}
