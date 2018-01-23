using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace IBALib.Types
{
    public class Color
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;
        
        public Vector3 Vector3 => new Vector3(R, G, B);
        public Vector4 Vector4 => new Vector4(Vector3, A);
        private int FloatToInt(float f) => (int)Math.Floor(f >= 1.0f ? 255 : f * 256.0);
        private float IntToFloat(int i) => i / 255.0f;

        public Color(float r, float g, float b) : this(r, g, b, 1.0f) { }

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int r, int g, int b) : this(r, g, b, 255) { }

        public Color(int r, int g, int b, int a)
        {
            R = IntToFloat(r);
            G = IntToFloat(g);
            B = IntToFloat(b);
            A = IntToFloat(a);
        }
    }
}
