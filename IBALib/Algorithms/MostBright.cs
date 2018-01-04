using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    internal class MostBright : IBlendAlgorithm
    {
        public string GetName()
        {
            return "MostBright";
        }
        public Color Calculate(IEnumerable<Color> colors )
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            return (a.R + a.G + a.B) / 3f >= (b.R + b.G + b.B) / 3 ? a : b;
        }

        public string GetVerboseName()
        {
            return "MostBright";
        }
    }
}
