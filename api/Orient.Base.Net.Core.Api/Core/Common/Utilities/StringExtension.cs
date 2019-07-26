using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Orient.Base.Net.Core.Api.Core.Common.Utilities
{
    public static class StringExtension
    {
        public static string[] SplitToArray(this string text, string separator)
        {
            string[] separators = null;
            if (string.IsNullOrEmpty(separator))
            {
                separators = new string[] { "," };
            }
            else
            {
                separators = new string[] { separator };
            }
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string CustomString(string str)
        {
            str = str.Trim();
            var arrayStr = str.Split(" ");

            string result = "";

            foreach (var text in arrayStr)
            {
                if(!text.Equals(""))
                {
                    result += text + " ";
                }
            }

            return result.TrimEnd();
        }
    }
}
