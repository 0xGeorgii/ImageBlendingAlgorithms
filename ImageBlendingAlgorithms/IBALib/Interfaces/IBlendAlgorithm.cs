using IBALib.Types;
using System;
using System.Collections.Generic;

namespace IBALib.Interfaces
{
    public interface IBlendAlgorithm : IAlgorithm
    {
        Colour Calculate(IEnumerable<Colour> colours);
    }
}
