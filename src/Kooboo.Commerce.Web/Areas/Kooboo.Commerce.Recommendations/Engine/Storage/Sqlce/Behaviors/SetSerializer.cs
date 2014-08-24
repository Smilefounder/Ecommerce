using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    static class SetSerializer
    {
        public static HashSet<string> Deserialize(string items)
        {
            if (String.IsNullOrEmpty(items))
            {
                return new HashSet<string>();
            }

            return new HashSet<string>(items.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string Serialize(IEnumerable<string> items)
        {
            return String.Join(",", items);
        }
    }
}