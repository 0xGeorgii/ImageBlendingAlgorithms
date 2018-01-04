using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    internal class AVGContrastCascade : IBlendAlgorithm
    {
        public string GetName()
        {
            return "AVGContrastCascade";
        }

        public Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            var r = 0.5f + ((a.R + b.R / 2f) - 0.5f) * 2f;
            if (r < 0) r = 0;
            else if (r > 1f) r = 1f;
            var g = 0.5f + ((a.G + b.G / 2f) - 0.5f) * 2f;
            if (g < 0) g = 0;
            else if (g > 1f) g = 1f;
            var _b = 0.5f + ((a.B + b.B / 2f) - 0.5f) * 2f;
            if (_b < 0) _b = 0;
            else if (_b > 1f) _b = 1f;
            return new Color(r, g, _b, 1f);
        }

        public string GetVerboseName()
        {
            return "ContrastCascade";
        }
    }
}
