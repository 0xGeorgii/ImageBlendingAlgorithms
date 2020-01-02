using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebUI.Utils
{
    public static class StringUtils
    {
        public static string SplitCamelCase(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "((AVG)|(BW)|([A-Z]))", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static bool EqualsIgnoreSpaces(this string input, string compareWith)
        {
            var cmp = Regex.Replace(compareWith, @"\s+", "");
            var inp = Regex.Replace(input, @"\s+", "");
            return inp.Equals(cmp, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
