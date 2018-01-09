using Microsoft.VisualStudio.TestTools.UnitTesting;
using IBALib;
using IBALib.Interfaces;
using IBALib.Types;
using System.Collections.Generic;

namespace IBALibTest
{
    [TestClass]
    public class UnitTest1
    {

        private static AlgorithmFactory factory = AlgorithmFactory.Instance;

        private Color colorGray = new Color(0.2f, 0.3f, 0.2f);
        private Color colorRed = new Color(0.5f, 0.0f, 0.0f);
        private Color colorPurple = new Color(0.5f, 0.0f, 0.1f);
        private Color colorLightGray = new Color(0.5f, 0.5f, 0.5f);
        private Color colorBlack = new Color(0.0f, 0.0f, 0.0f);
        private Color colorYellow = new Color(0.7f, 0.7f, 0.0f);

        private void CompareTwoColor(Color color1, Color color2)
        {
            string str1 = string.Format("R: {0}, G: {1}, B:{2}", color1.R, color1.G, color1.B);
            string str2 = string.Format("R: {0}, G: {1}, B:{2}", color2.R, color2.G, color2.B);
            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void Glass()
        {
            IBlendAlgorithm glass = factory.GetAlgorithmByName("Glass");

            List<Color> input = new List<Color> { colorGray, colorRed };
            CompareTwoColor(new Color(0.35f, 0.15f, 0.1f), glass.Calculate(input));

            input = new List<Color> { colorPurple, colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.05f), glass.Calculate(input));

            input = new List<Color> { colorPurple, colorGray };
            CompareTwoColor(new Color(0.35f, 0.15f, 0.15f), glass.Calculate(input));

            input.Clear();

        }

        [TestMethod]
        public void AvgContrast()
        {
            IBlendAlgorithm avgContrast = factory.GetAlgorithmByName("Contrast");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input = new List<Color> { colorYellow, colorRed, colorBlack, colorLightGray };
            CompareTwoColor(new Color(0.35f, 0.1f, 0.0f), avgContrast.Calculate(input));

            input = new List<Color> { colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), avgContrast.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void AvgContrastCascade()
        {
            IBlendAlgorithm avgContrastCascade = factory.GetAlgorithmByName("AVGContrastCascade");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Color> { colorYellow, colorRed, colorBlack, colorLightGray };
            CompareTwoColor(new Color(1.0f, 0.9f, 0.0f), avgContrastCascade.Calculate(input));

            input = new List<Color> { colorRed, colorRed };
            CompareTwoColor(new Color(1.0f, 0.0f, 0.0f), avgContrastCascade.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBright()
        {
            IBlendAlgorithm mostBright = factory.GetAlgorithmByName("MostBright");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostBright.Calculate(input));

            input = new List<Color> { colorRed, colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBright.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostBrightWithTreshold()
        {
            IBlendAlgorithm mostBrightWT = factory.GetAlgorithmByName("MostBrightWithTreshold");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostBrightWT.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostBrightWT.Calculate(input));

            input = new List<Color> { colorYellow, colorYellow };
            CompareTwoColor(new Color(0.65f, 0.65f, 0.0f), mostBrightWT.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostColorful()
        {
            IBlendAlgorithm mostColorful = factory.GetAlgorithmByName("Color");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostColorful.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(0.7f, 0.7f, 0.0f), mostColorful.Calculate(input));

            input = new List<Color> { colorRed, colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostColorful.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostContrastBW()
        {
            IBlendAlgorithm mostContrastBW = factory.GetAlgorithmByName("MostContrastBW");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(0.5f, 0.5f, 0.5f), mostContrastBW.Calculate(input));

            input = new List<Color> { colorRed, colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostContrastBW.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDark()
        {
            IBlendAlgorithm mostDark = factory.GetAlgorithmByName("MostDark");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(0.7f, 0.7f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { colorRed, colorPurple };
            CompareTwoColor(new Color(0.5f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

        [TestMethod]
        public void MostDarkWithTreshold()
        {
            IBlendAlgorithm mostDark = factory.GetAlgorithmByName("MostDarkWithTreshold");

            List<Color> input = new List<Color> { colorBlack, colorRed };
            CompareTwoColor(new Color(0.0f, 0.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { colorYellow, colorLightGray };
            CompareTwoColor(new Color(1.0f, 1.0f, 0.0f), mostDark.Calculate(input));

            input = new List<Color> { colorRed, colorPurple };
            CompareTwoColor(new Color(0.75f, 0.0f, 0.0f), mostDark.Calculate(input));

            input.Clear();
        }

    }
}
