using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class ParameterDescription
    {
        public string Name { get; private set; }

        public Type ValueType { get; private set; }

        public object DefaultValue { get; private set; }

        public ParameterDescription(string name, Type valueType)
            : this(name, valueType, null)
        {
        }

        public ParameterDescription(string name, Type valueType, object defaultValue)
        {
            Name = name;
            ValueType = valueType;
            DefaultValue = defaultValue;
        }
    }

    public class Int32ParameterDescription : ParameterDescription
    {
        public Int32ParameterDescription(string name)
            : base(name, typeof(Int32))
        {
        }

        public Int32ParameterDescription(string name, int defaultValue)
            : base(name, typeof(Int32), defaultValue)
        {
        }
    }

    public class StringParameterDescription : ParameterDescription
    {
        public StringParameterDescription(string name)
            : base(name, typeof(String))
        {
        }

        public StringParameterDescription(string name, string defaultValue)
            : base(name, typeof(String), defaultValue)
        {
        }
    }
}
