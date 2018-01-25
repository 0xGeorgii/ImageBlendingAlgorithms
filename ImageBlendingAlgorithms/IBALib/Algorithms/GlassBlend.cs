using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.Algorithms
{
    [ImageBlendingAlgorithm]
    internal class GlassBlend : IBlendAlgorithm
    {
        public float Alpha;
        public int ImagesCount;

        public GlassBlend() : this(1f, 2)
        {
        }

        public GlassBlend(float alpha, int imagesCount)
        {
            Alpha = alpha;
            ImagesCount = imagesCount;
        }
        public Color Calculate(IEnumerable<Color> colors)
        {
            var divider = 1.0f / ImagesCount;
            float 
                sumR = colors.Sum(c=>c.R),
                sumG = colors.Sum(c=>c.G),
                sumB = colors.Sum(c=>c.B);
            return new Color(sumR * divider, sumG * divider, sumB * divider, Alpha);
        }

        public string GetName()
        {
            return "Glass";
        }

        public string GetVerboseName()
        {
            return "Pure";
        }
    }
}
