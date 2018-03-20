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

        private readonly Colour _ColourGray = new Colour(0.2f, 0.3f, 0.2f);
        private readonly Colour _ColourRed = new Colour(0.5f, 0.0f, 0.0f);
        private readonly Colour _ColourPurple = new Colour(0.5f, 0.0f, 0.1f);
        private readonly Colour _ColourLightGray = new Colour(0.5f, 0.5f, 0.5f);
        private readonly Colour _ColourBlack = new Colour(0.0f, 0.0f, 0.0f);
        private readonly Colour _ColourYellow = new Colour(0.7f, 0.7f, 0.0f);

        private void CompareTwoColour(Colour Colour1, Colour Colour2)
        {
            var str1 = $"R: {Colour1.R}, G: {Colour1.G}, B:{Colour1.B}";
            var str2 = $"R: {Colour2.R}, G: {Colour2.G}, B:{Colour2.B}";
            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void Glass_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm glass = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.GLASS];

            List<Colour> input = new List<Colour> { _ColourGray, _ColourRed };
            CompareTwoColour(new Colour(0.35f, 0.15f, 0.1f), glass.Calculate(input));

            input = new List<Colour> { _ColourPurple, _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.05f), glass.Calculate(input));

            input = new List<Colour> { _ColourPurple, _ColourGray };
            CompareTwoColour(new Colour(0.35f, 0.15f, 0.15f), glass.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void AvgContrast_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm avgContrast = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.AVGContrast];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.0f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourRed, _ColourBlack, _ColourLightGray };
            CompareTwoColour(new Colour(0.35f, 0.1f, 0.0f), avgContrast.Calculate(input));

            input = new List<Colour> { _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void AvgContrastCascade_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm avgContrastCascade = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.AVGContrastCascade];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourRed, _ColourBlack, _ColourLightGray };
            CompareTwoColour(new Colour(1.0f, 0.9f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourRed };
            CompareTwoColour(new Colour(1.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBright_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostBright = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostBright];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(0.5f, 0.5f, 0.5f), mostBright.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBrightWithTreshold_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostBrightWT = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostBrightWT];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostBrightWT.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(0.5f, 0.5f, 0.5f), mostBrightWT.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourYellow };
            CompareTwoColour(new Colour(0.65f, 0.65f, 0.0f), mostBrightWT.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostColourful_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostColourful = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostColourful];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostColourful.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(0.7f, 0.7f, 0.0f), mostColourful.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourPurple };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostColourful.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostContrastBW_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostContrastBW = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostContrastBW];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.0f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(0.5f, 0.5f, 0.5f), mostContrastBW.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourPurple };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDark_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostDark = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostDark];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(0.7f, 0.7f, 0.0f), mostDark.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourPurple };
            CompareTwoColour(new Colour(0.5f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDarkWithTreshold_PixelCalculatingWorksAsExpected()
        {
            IBlendAlgorithm mostDark = _factory.BlendingAlgorithmsDictionary[AlgorithmFactory.ALGORITHM.MostDarkWT];

            List<Colour> input = new List<Colour> { _ColourBlack, _ColourRed };
            CompareTwoColour(new Colour(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Colour> { _ColourYellow, _ColourLightGray };
            CompareTwoColour(new Colour(1.0f, 1.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Colour> { _ColourRed, _ColourPurple };
            CompareTwoColour(new Colour(0.75f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

    }
}
