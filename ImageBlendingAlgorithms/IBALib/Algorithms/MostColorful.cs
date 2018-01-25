using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class MostColorful : IBlendAlgorithm
    {
        public string GetName()
        {
            return "Color";
        }

        public Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            var aq = Math.Abs(getU(a) - 128) + Math.Abs(getV(a) - 128);
            var bq = Math.Abs(getU(b) - 128) + Math.Abs(getV(b) - 128);
            return aq > bq ? a : b;
        }

        static double getY(Color c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        static double getU(Color c)
        {
            return -0.14713 * c.R - 0.28886 * c.G + 0.436 * c.B + 128;
        }
        static double getV(Color c)
        {
            return 0.615 * c.R - 0.51499 * c.G - 0.10001 * c.B + 128;
        }

        public string GetVerboseName()
        {
            return "MostColorful";
        }
    }
}