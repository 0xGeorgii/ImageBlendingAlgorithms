using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SourceProvider.Configuration
{
    public class UrlProvider
    {
        private static readonly HashSet<string> _urls = new HashSet<string> 
        {            
            "https://picsum.photos/{x}/{y}/?random",
            "http://lorempixel.com/{x}/{y}"
        };

        public static string GetRandomUrl(uint x, uint y) => FormatString(x, y, _urls.ToArray()[new Random(DateTime.UtcNow.Millisecond).Next(0, _urls.Count - 1)]);
        private static string FormatString(uint x, uint y, string src) => src.Replace("{x}", x.ToString()).Replace("{y}", y.ToString());
    }
}
