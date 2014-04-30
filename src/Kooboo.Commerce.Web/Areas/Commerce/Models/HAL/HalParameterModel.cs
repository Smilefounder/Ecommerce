using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class HalParameterModel
    {
        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
                if (!String.IsNullOrEmpty(Name))
                {
                    var index = Name.IndexOf('.');
                    if (index >= 0)
                    {
                        return Name.Substring(index + 1);
                    }
                }

                return Name;
            }
        }

        public bool Required { get; set; }

        public string ParameterType { get; set; }

        public HalParameterModel() { }

        public HalParameterModel(HalParameter param)
        {
            Name = param.Name;
            Required = param.Required;
            ParameterType = param.ParameterType.Name;
        }
    }
}