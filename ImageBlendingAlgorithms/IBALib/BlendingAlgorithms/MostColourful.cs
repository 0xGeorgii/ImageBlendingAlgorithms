using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.BlendingAlgorithms
{
    [ImageBlendingAlgorithm]
    internal class MostColourful : AIBAlgorithm
    {
        public override Colour Calculate(IEnumerable<Colour> colours)
        {
            var a = colours.ElementAt(0);
            var b = colours.ElementAt(1);
            var aq = Math.Abs(GetU(a) - 128) + Math.Abs(GetV(a) - 128);
            var bq = Math.Abs(GetU(b) - 128) + Math.Abs(GetV(b) - 128);
            return aq > bq ? a : b;
        }

        private static double GetY(Colour c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        private static double GetU(Colour c)
        {
            return -0.14713 * c.R - 0.28886 * c.G + 0.436 * c.B + 128;
        }

        private static double GetV(Colour c)
        {
            return 0.615 * c.R - 0.51499 * c.G - 0.10001 * c.B + 128;
        }

        public override string GetVerboseName() => "Most Colourful";
    }
}