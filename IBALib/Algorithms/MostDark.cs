using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class MostDark : IBlendAlgorithm
    {
        public string GetName()
        {
            return "MostDark";
        }

        public Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            return (a.R + a.G + a.B) / 3f >= (b.R + b.G + b.B) / 3f ? b : a;
        }

        public string GetVerboseName()
        {
            return "MostDark";
        }
    }
}
