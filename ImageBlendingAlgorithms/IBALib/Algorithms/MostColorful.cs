using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class MostColorful : AIBAlgorithm
    {
        public override Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            var aq = Math.Abs(GetU(a) - 128) + Math.Abs(GetV(a) - 128);
            var bq = Math.Abs(GetU(b) - 128) + Math.Abs(GetV(b) - 128);
            return aq > bq ? a : b;
        }

        private static double GetY(Color c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        private static double GetU(Color c)
        {
            return -0.14713 * c.R - 0.28886 * c.G + 0.436 * c.B + 128;
        }

        private static double GetV(Color c)
        {
            return 0.615 * c.R - 0.51499 * c.G - 0.10001 * c.B + 128;
        }

        public override string GetVerboseName() => "Most Colorful";
    }
}