using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.BlendingAlgorithms
{
    [ImageBlendingAlgorithm]
    internal class MostBright : AIBAlgorithm
    {
        public override Colour Calculate(IEnumerable<Colour> colours )
        {
            var a = colours.ElementAt(0);
            var b = colours.ElementAt(1);
            return (a.R + a.G + a.B) / 3f >= (b.R + b.G + b.B) / 3 ? a : b;
        }

        public override string GetVerboseName() => "Most Bright";
    }
}
