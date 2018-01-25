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
        private int FloatToByte(float f) => (int)Math.Floor(f >= 1.0f ? byte.MaxValue : f * 256.0);
        private float ByteToFloat(byte i) => i / 255.0f;

        public Color(float r, float g, float b) : this(r, g, b, 1.0f) { }

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

        public Color(byte r, byte g, byte b, byte a)
        {
            R = ByteToFloat(r);
            G = ByteToFloat(g);
            B = ByteToFloat(b);
            A = ByteToFloat(a);
        }
    }
}
