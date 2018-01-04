using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBALib.Types
{
    public class Color
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public Color(float r, float g, float b) : this(r, g, b, 1)
        {
            this.R = r;
            G = g;
            B = b;
        }

        public Color(float r, float g, float b, float a)
        {
            this.R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
