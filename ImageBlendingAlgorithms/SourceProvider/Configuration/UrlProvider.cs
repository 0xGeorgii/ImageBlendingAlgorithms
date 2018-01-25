using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SourceProvider.Configuration
{
    public class UrlProvider
    {
        private static readonly string SETTINGS_FILE = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\settings.json";
        private static readonly HashSet<string> _urls = new HashSet<string>();

        static UrlProvider()
        {
            using (var sr = new StreamReader(SETTINGS_FILE))
            {
                var settings = sr.ReadToEnd();
                var setttingsObject = JsonConvert.DeserializeObject<Settings>(settings);
                foreach (string url in setttingsObject.ImageProvidersUrls)
                {
                    _urls.Add(url);
                }
            }
        }

        public static string GetRandomUrl(uint x, uint y) => FormatString(x, y, _urls.ToArray()[new Random(DateTime.UtcNow.Millisecond).Next(0, _urls.Count - 1)]);
        private static string FormatString(uint x, uint y, string src) => src.Replace("{x}", x.ToString()).Replace("{y}", y.ToString());
    }
}
