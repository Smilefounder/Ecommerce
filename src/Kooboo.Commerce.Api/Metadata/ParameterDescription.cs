using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Metadata
{
    public class ParameterDescription
    {
        public string Name { get; private set; }

        public Type ValueType { get; private set; }

        public bool Required { get; private set; }

        public ParameterDescription(string name, Type valueType, bool required)
        {
            Name = name;
            ValueType = valueType;
            Required = required;
        }
    }

    public class Int32ParameterDescription : ParameterDescription
    {
        public Int32ParameterDescription(string name, bool required)
            : base(name, typeof(Int32), required)
        {
        }
    }

    public class StringParameterDescription : ParameterDescription
    {
        public StringParameterDescription(string name, bool required)
            : base(name, typeof(String), required)
        {
        }
    }
}
