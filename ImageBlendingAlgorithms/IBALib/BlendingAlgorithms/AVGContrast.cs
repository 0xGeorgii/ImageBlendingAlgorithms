using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.BlendingAlgorithms
{
    [ImageBlendingAlgorithm]
    internal class AVGContrast : AIBAlgorithm
    {
        public override Color Calculate(IEnumerable<Color> colors)
        {
            var r = 0.5f + (colors.Average(c => c.R) - 0.5f) * 2f;
            if (r < 0) r = 0;
            else if (r > 1f) r = 1f;
            var g = 0.5f + (colors.Average(c => c.G) - 0.5f) * 2f;
            if (g < 0) g = 0;
            else if (g > 1f) g = 1f;
            var _b = 0.5f + (colors.Average(c => c.B) - 0.5f) * 2f;
            if (_b < 0) _b = 0;
            else if (_b > 1f) _b = 1f;
            return new Color(r, g, _b, 1f);
        }

        public override string GetVerboseName() => "Average Contrast";
    }
}
