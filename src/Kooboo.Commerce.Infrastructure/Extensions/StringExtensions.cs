using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class StringExtensions
    {
        public static string Humanize(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var result = new StringBuilder();
            var isLastCharUpper = false;

            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    if (i > 0 && !isLastCharUpper)
                    {
                        result.Append(" ");
                    }

                    isLastCharUpper = true;
                }
                else
                {
                    isLastCharUpper = false;
                }

                result.Append(ch);
            }

            return result.ToString();
        }
    }
}
