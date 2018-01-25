using IBALib.Types;
using System;
using System.Collections.Generic;

namespace IBALib.Interfaces
{
    public interface IBlendAlgorithm
    {
        Color Calculate(IEnumerable<Color> colors);
        string GetName();
        string GetVerboseName();
    }
}