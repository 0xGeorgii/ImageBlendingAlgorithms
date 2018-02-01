using IBALib.Types;
using System;
using System.Collections.Generic;

namespace IBALib.Interfaces
{
    public interface IBlendAlgorithm : IAlgorithm
    {
        Color Calculate(IEnumerable<Color> colors);
    }
}
