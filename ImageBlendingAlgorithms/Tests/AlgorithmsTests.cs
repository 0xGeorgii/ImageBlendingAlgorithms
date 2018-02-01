using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBALib;
using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;

namespace IBALibTest
{
    [TestClass]
    public class AlgorithmsTests
    {
        private static readonly AlgorithmFactory _factory = AlgorithmFactory.Instance;

        private readonly Color _colorGray = new Color(0.2f, 0.3f, 0.2f);
        private readonly Color _colorRed = new Color(0.5f, 0.0f, 0.0f);
        private readonly Color _colorPurple = new Color(0.5f, 0.0f, 0.1f);
        private readonly Color _colorLightGray = new Color(0.5f, 0.5f, 0.5f);
        private readonly Color _colorBlack = new Color(0.0f, 0.0f, 0.0f);
        private readonly Color _colorYellow = new Color(0.7f, 0.7f, 0.0f);

        private void CompareTwoColor(Color color1, Color color2)
        {
            var str1 = $"R: {color1.R}, G: {color1.G}, B:{color1.B}";
            var str2 = $"R: {color2.R}, G: {color2.G}, B:{color2.B}";
            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void Glass_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm glass = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.GLASS];

            List<Color> input = new List<Color> { _colorGray, _colorRed };
            CompareTwoColor(new Color(0.35f, 0.15f, 0.1f), glass.Calculate(input));

            input = new List<Color> { _colorPurple, _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.05f), glass.Calculate(input));

            input = new List<Color> { _colorPurple, _colorGray };
            CompareTwoColor(new Color(0.35f, 0.15f, 0.15f), glass.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void AvgContrast_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm avgContrast = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.AVGContrast];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input = new List<Color> { _colorYellow, _colorRed, _colorBlack, _colorLightGray };
            CompareTwoColor(new Color(0.35f, 0.1f, 0.0f), avgContrast.Calculate(input));

            input = new List<Color> { _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void AvgContrastCascade_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm avgContrastCascade = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.AVGContrastCascade];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Color> { _colorYellow, _colorRed, _colorBlack, _colorLightGray };
            CompareTwoColor(new Color(1.0f, 0.9f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Color> { _colorRed, _colorRed };
            CompareTwoColor(new Color(1.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBright_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostBright = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostBright];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostBright.Calculate(input));

            input = new List<Color> { _colorRed, _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBrightWithTreshold_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostBrightWT = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostBrightWT];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBrightWT.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostBrightWT.Calculate(input));

            input = new List<Color> { _colorYellow, _colorYellow };
            CompareTwoColor(new Color(0.65f, 0.65f, 0.0f), mostBrightWT.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostColorful_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostColorful = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostColorful];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostColorful.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(0.7f, 0.7f, 0.0f), mostColorful.Calculate(input));

            input = new List<Color> { _colorRed, _colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostColorful.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostContrastBW_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostContrastBW = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostContrastBW];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostContrastBW.Calculate(input));

            input = new List<Color> { _colorRed, _colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDark_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostDark = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostDark];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(0.7f, 0.7f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { _colorRed, _colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDarkWithTreshold_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostDark = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostDarkWT];

            List<Color> input = new List<Color> { _colorBlack, _colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { _colorYellow, _colorLightGray };
            CompareTwoColor(new Color(1.0f, 1.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { _colorRed, _colorPurple };
            CompareTwoColor(new Color(0.75f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

    }
}
