using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HalParameterProviderAttribute : Attribute
    {
        public HalParameterProviderAttribute()
        { }

        public HalParameterProviderAttribute(string[] forResources)
        {
            ForResources = ForResources;
        }

        public string[] ForResources { get; set; }
    }
}
