using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web
{
    public static class TimeSpanExtensions
    {
        public static string Humanize(this TimeSpan timespan)
        {
            var parts = new List<string>();
            if (timespan.Days > 0)
            {
                parts.Add(timespan.Days + " day" + (timespan.Days > 1 ? "s" : ""));
            }
            if (timespan.Hours > 0)
            {
                parts.Add(timespan.Hours + " hour" + (timespan.Hours > 1 ? "s" : ""));
            }
            if (timespan.Minutes > 0)
            {
                parts.Add(timespan.Minutes + " minute" + (timespan.Minutes > 1 ? "s" : ""));
            }
            if (timespan.Seconds > 0)
            {
                parts.Add(timespan.Seconds + " second" + (timespan.Seconds > 1 ? "s" : ""));
            }

            return String.Join(", ", parts);
        }
    }
}