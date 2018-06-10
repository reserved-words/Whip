using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string GetFirstWord(this string str)
        {
            var firstSpace = str.IndexOf(" ");
            if (firstSpace <= 0)
                return str;

            return str.Substring(0, firstSpace);
        }

        public static bool IsInteger(this string str)
        {
            int i;
            return int.TryParse(str, out i);
        }
    }
}
