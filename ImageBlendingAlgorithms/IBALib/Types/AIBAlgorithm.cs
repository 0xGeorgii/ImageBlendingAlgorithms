using IBALib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBALib.Types
{
    internal abstract class AIBAlgorithm : IBlendAlgorithm
    {
        public abstract Color Calculate(IEnumerable<Color> colors);

        public virtual string GetName()
        {
            return GetType().Name;
        }

        public abstract string GetVerboseName();
    }
}
