using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class HalParameterAttribute : Attribute
    {
        public HalParameterAttribute()
        {
            Required = false;
        }

        public HalParameterAttribute(string name, Type parameterType, bool required = false)
        {
            Name = name;
            ParameterType = parameterType;
            Required = required;
        }

        public string Name { get; set; }
        public Type ParameterType { get; set; }
        public bool Required { get; set; }
    }
}
