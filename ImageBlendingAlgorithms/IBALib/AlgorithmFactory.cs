using IBALib.BlendingAlgorithms;
using IBALib.Interfaces;
using IBALib.ScalingAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;

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
            MostContrastBW,
            NearestNeighbor,
            NearestNeighborDownscale
        };

        public Dictionary<ALGORITHM, IBlendAlgorithm> BlendingAlgorithmsDictionary = new Dictionary<ALGORITHM, IBlendAlgorithm>()
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

        public Dictionary<ALGORITHM, IScaleAlgorithm> ScalingAlgorithmsDictionary = new Dictionary<ALGORITHM, IScaleAlgorithm>()
        {
            { ALGORITHM.NearestNeighbor, new NearestNeighbor() },
            { ALGORITHM.NearestNeighborDownscale, new NearestNeighborDownscale() }
        };

        public List<IBlendAlgorithm> BlendingAlgorithms = new List<IBlendAlgorithm>()
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

        public List<IScaleAlgorithm> ScalingAlgorithms = new List<IScaleAlgorithm>()
        {
            new NearestNeighbor(),
            new NearestNeighborDownscale()
        };


        static AlgorithmFactory()
        {

        }

        private AlgorithmFactory() { }

        public IBlendAlgorithm GetRandomBlendingAlgorithm()
        {
            return BlendingAlgorithms[new Random(DateTime.UtcNow.Millisecond).Next(0, BlendingAlgorithms.Count - 1)];
        }

        public IBlendAlgorithm GetBlendingAlgorithmByName(string name)
        {
            return BlendingAlgorithms.FirstOrDefault(item => item.GetName().Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
