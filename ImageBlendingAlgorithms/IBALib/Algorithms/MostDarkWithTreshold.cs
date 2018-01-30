using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class MostDarkWithTreshold : AIBAlgorithm
    {
        private float _threshold = 0.25f;
        
        public override Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            var c = (a.R + a.G + a.B) / 3f <= (b.R + b.G + b.B) / 3f ? a : b;

            var r = 0.5f + (c.R - 0.5f) * 2f;
            if (r < 0) r = 0;
            if (r > _threshold)
            {
                r += 0.25f;
                if (r > 1f) r = 1f;
                if (r < 0) r = 0;
            }

            var g = 0.5f + (c.G - 0.5f) * 2f;
            if (g < 0) g = 0;
            if (g > _threshold)
            {
                g += 0.25f;
                if (g > 1f) g = 1f;
                if (g < 0) g = 0;
            }

            var _b = 0.5f + (c.B - 0.5f) * 2f;
            if (_b < 0) _b = 0;
            if (!(_b > _threshold)) return new Color(r, g, _b, 1f);
            _b += 0.25f;
            if (_b > 1f) _b = 1f;
            if (_b < 0) _b = 0;

            return new Color(r, g, _b, 1f);
        }

        public override string GetVerboseName() => "Most Dark with Treshold";
    }
}
