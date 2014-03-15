using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IConditionParameter
    {
        string Name { get; }

        string DisplayName { get; }

        Type ModelType { get; }

        Type ValueType { get; }

        IEnumerable<IComparisonOperator> SupportedOperators { get; }

        object GetValue(object model);

        object ParseValue(string value);
    }
}
