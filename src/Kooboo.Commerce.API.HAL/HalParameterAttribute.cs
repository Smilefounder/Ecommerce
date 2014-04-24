using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HalParameterAttribute : Attribute
    {
        public HalParameterAttribute()
        {
            Required = null;
        }

        public HalParameterAttribute(string name, Type parameterType, bool? required = null)
        {
            Name = name;
            ParameterType = parameterType;
            Required = required;
        }

        public string Name { get; set; }
        public Type ParameterType { get; set; }
        public bool? Required { get; set; }
    }
}
