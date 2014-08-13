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
}
