using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;
using System.Linq;

namespace IBALib.BlendingAlgorithms
{
    [ImageBlendingAlgorithm]
    internal class GlassBlend : AIBAlgorithm
    {
        private float _alpha;
        private int _imagesCount;

        public GlassBlend() : this(1f, 2)
        {
        }

        public GlassBlend(float alpha, int imagesCount)
        {
            _alpha = alpha;
            _imagesCount = imagesCount;
        }

        public override Color Calculate(IEnumerable<Color> colors)
        {
            var divider = 1.0f / _imagesCount;
            float 
                sumR = colors.Sum(c=>c.R),
                sumG = colors.Sum(c=>c.G),
                sumB = colors.Sum(c=>c.B);
            return new Color(sumR * divider, sumG * divider, sumB * divider, _alpha);
        }

        public override string GetVerboseName() => "Glass Blend";
    }
}
