using System;
using System.Text.RegularExpressions;

namespace WebApiFssp.Helpers
{
    public static class RegexpHelpers
    {
        public static string GetDataFromRegex(this string s, string r)
        {
            var ret = "";
            var regex = new Regex(r, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regex.Matches(s);
            if (matches.Count > 0)
            {
                ret = matches[0].Groups[1].Value.Trim();
            }

            return ret;
        }
    }
}