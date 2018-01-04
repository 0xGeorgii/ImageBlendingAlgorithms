using IBALib.Interfaces;
using IBALib.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    internal class MostContrastBW : IBlendAlgorithm
    {
        public string GetName()
        {
            return "MostContrastBW";
        }

        public Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            var aY = Math.Abs(128 - getY(a));
            var bY = Math.Abs(128 - getY(b));
            return aY > bY ? a : b;
        }

        static double getY(Color c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        public string GetVerboseName()
        {
            return "B/W";
        }
    }
}
