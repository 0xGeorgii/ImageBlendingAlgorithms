using System.Collections.Generic;

namespace IBALib
{
    public interface IBlendAlgorithm
    {
        Color Calculate(IEnumerable<Color> colors);
        string GetName();
        string GetVerboseName();
    }
}