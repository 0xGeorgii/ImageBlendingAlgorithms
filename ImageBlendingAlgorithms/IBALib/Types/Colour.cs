using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace IBALib.Types
{
    [Serializable]
    public struct Colour
    {
        [NonSerialized]
        public float R;
        [NonSerialized]
        public float G;
        [NonSerialized]
        public float B;
        [NonSerialized]
        public float A;
        
        public uint _rawData;
                
        public Vector3 Vector3 => new Vector3(R, G, B);
        public Vector4 Vector4 => new Vector4(Vector3, A);
        private static byte FloatToByte(float f) => (byte)Math.Floor(f >= 1.0f ? byte.MaxValue : f * 256.0);
        private static float ByteToFloat(byte i) => i / 255.0f;
        public void UpdateRawData()
        {
            _rawData = 0;
            _rawData >>= 24;
            _rawData |= FloatToByte(R);
            _rawData <<= 24;
            _rawData >>= 16;
            _rawData |= FloatToByte(G);
            _rawData <<= 16;
            _rawData >>= 8;
            _rawData |= FloatToByte(B);
            _rawData <<= 8;
            _rawData |= FloatToByte(A);
        }

        public Colour(float r, float g, float b) : this(r, g, b, 1.0f) { }

        public Colour(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            _rawData = 0;
        }

        public Colour(byte r, byte g, byte b) : this(r, g, b, 255) { }

        public Colour(byte r, byte g, byte b, byte a)
        {
            R = ByteToFloat(r);
            G = ByteToFloat(g);
            B = ByteToFloat(b);
            A = ByteToFloat(a);
            _rawData = 0;
        }
        
        public void FillFrom(Colour c)
        {
            R = c.R;
            G = c.G;
            B = c.B;
            A = c.A;
        }

        public static Colour FromObject(object obj)
        {
            return new Colour
            {
                R = ByteToFloat((byte)obj.GetType().GetField("R").GetValue(obj)),
                G = ByteToFloat((byte)obj.GetType().GetField("G").GetValue(obj)),
                B = ByteToFloat((byte)obj.GetType().GetField("B").GetValue(obj)),
                A = ByteToFloat((byte)obj.GetType().GetField("A").GetValue(obj))
            };
        }
    }
}
