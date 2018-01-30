using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class MostDark : AIBAlgorithm
    {
        public override Color Calculate(IEnumerable<Color> colors)
        {
            var a = colors.ElementAt(0);
            var b = colors.ElementAt(1);
            return (a.R + a.G + a.B) / 3f >= (b.R + b.G + b.B) / 3f ? b : a;
        }

        public override string GetVerboseName() => "Most Dark";
    }
}
