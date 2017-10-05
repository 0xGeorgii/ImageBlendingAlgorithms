using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBALib
{
    public class AlgorithmFactory
    {
        public static AlgorithmFactory Instance = new AlgorithmFactory();
        public enum ALGORITHM
        {
            GLASS,
            AVGContrast,
            AVGContrastCascade,
            MostBright,
            MostBrightWT,
            MostDark,
            MostDarkWT,
            MostColorful,
            MostContrastBW
        };

        public Dictionary<ALGORITHM, IBlendAlgorithm> AlgorithmsDictionary = new Dictionary<ALGORITHM, IBlendAlgorithm>()
        {
            { ALGORITHM.GLASS, new GlassBlend() },
            { ALGORITHM.AVGContrast, new AVGContrast() },
            { ALGORITHM.AVGContrastCascade, new AVGContrastCascade() },
            { ALGORITHM.MostBright, new MostBright() },
            { ALGORITHM.MostBrightWT, new MostBrightWithTreshold() },
            { ALGORITHM.MostDark, new MostDark() },
            { ALGORITHM.MostDarkWT, new MostDarkWithTreshold() },
            { ALGORITHM.MostColorful, new MostColorful() },
            { ALGORITHM.MostContrastBW, new MostContrastBW() }
        };

        public List<IBlendAlgorithm> Algorithms = new List<IBlendAlgorithm>()
        {
            new GlassBlend(),
            new AVGContrast(),
            new AVGContrastCascade(),
            new MostBright(),
            new MostBrightWithTreshold(),
            new MostDark(),
            new MostDarkWithTreshold(),
            new MostColorful(),
            new MostContrastBW()
        };

        static AlgorithmFactory()
        {

        }

        private AlgorithmFactory() { }

        public IBlendAlgorithm GetRandomAlgorithm()
        {
            return Algorithms[new Random(DateTime.Now.Millisecond).Next(0, Algorithms.Count - 1)];
        }

        public IBlendAlgorithm GetAlgorithmByName(string name)
        {
            return Algorithms.FirstOrDefault(item => item.GetName().Equals(name));
        }
    }
}
