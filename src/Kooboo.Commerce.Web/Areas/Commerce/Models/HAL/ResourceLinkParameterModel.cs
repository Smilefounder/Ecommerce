using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class ResourceLinkParameterModel
    {
        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
                return String.IsNullOrEmpty(Name) ? Name : GetParameterDisplayName(Name);
            }
        }

        public string Value { get; set; }

        public bool UseFixedValue { get; set; }

        public bool Required { get; set; }

        public string ParameterType { get; set; }

        public static string GetParameterDisplayName(string paramName)
        {
            var index = paramName.IndexOf('.');
            if (index >= 0)
            {
                return paramName.Substring(index + 1);
            }

            return paramName;
        }
    }
}