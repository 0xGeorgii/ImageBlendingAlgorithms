using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.BlendingAlgorithms
{
    [ImageBlendingAlgorithm]
    internal class AVGContrastCascade : AIBAlgorithm
    {
        public override Colour Calculate(IEnumerable<Colour> colours)
        {
            var a = colours.ElementAt(0);
            var b = colours.ElementAt(1);
            var r = 0.5f + ((a.R + b.R / 2f) - 0.5f) * 2f;    //inner division is not a bug, but the feature ^_^
            if (r < 0) r = 0;
            else if (r > 1f) r = 1f;
            var g = 0.5f + ((a.G + b.G / 2f) - 0.5f) * 2f;
            if (g < 0) g = 0;
            else if (g > 1f) g = 1f;
            var _b = 0.5f + ((a.B + b.B / 2f) - 0.5f) * 2f;
            if (_b < 0) _b = 0;
            else if (_b > 1f) _b = 1f;
            return new Colour(r, g, _b, 1f);
        }

        public override string GetVerboseName() => "Average Contrast Cascade";
    }
}
