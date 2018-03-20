using IBALib.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IBALib.Types
{
    internal abstract class AIBAlgorithm : IBlendAlgorithm
    {
        public abstract Colour Calculate(IEnumerable<Colour> colours);

        public virtual string GetName() => GetType().Name;

        public abstract string GetVerboseName();
    }
}
