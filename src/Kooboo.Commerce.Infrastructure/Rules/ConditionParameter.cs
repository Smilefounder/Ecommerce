using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ConditionParameter
    {
        public string Name { get; private set; }

        public Type ValueType { get; private set; }

        public IParameterValueResolver ValueResolver { get; private set; }

        public IParameterValueSource ValueSource { get; private set; }

        public IList<IComparisonOperator> SupportedOperators { get; private set; }

        public ConditionParameter(string name, Type valueType, IParameterValueResolver valueResolver, IEnumerable<IComparisonOperator> supportedOperators)
            : this(name, valueType, valueResolver, null, supportedOperators)
        {
        }

        public ConditionParameter(string name, Type valueType, IParameterValueResolver valueResolver, IParameterValueSource valueSource, IEnumerable<IComparisonOperator> supportedOperators)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(valueType, "valueType");
            Require.NotNull(valueResolver, "valueResolver");
            Require.NotNull(supportedOperators, "supportedOperators");

            Name = name;
            ValueType = valueType;
            ValueResolver = valueResolver;
            ValueSource = valueSource;
            SupportedOperators = supportedOperators.ToList();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
