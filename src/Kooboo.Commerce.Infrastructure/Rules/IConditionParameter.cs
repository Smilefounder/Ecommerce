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

        ParameterValueType ValueType { get; }

        IEnumerable<IComparisonOperator> SupportedOperators { get; }

        object GetValue(object model);
    }

    public enum ParameterValueType
    {
        String = 0,
        Integer = 1,
        Number = 2
    }

    public static class ParameterValueTypeExtensions
    {
        public static Type GetClrType(this ParameterValueType type)
        {
            if (type == ParameterValueType.String)
            {
                return typeof(string);
            }
            if (type == ParameterValueType.Integer)
            {
                return typeof(Int32);
            }
            if (type == ParameterValueType.Number)
            {
                return typeof(double);
            }

            throw new NotSupportedException();
        }
    }
}
