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

    static class ParameterValueTypeUtil
    {
        public static Type ToClrType(this ParameterValueType type)
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

        public static ParameterValueType ToParameterValueType(this Type type)
        {
            if (type == typeof(String))
            {
                return ParameterValueType.String;
            }
            if (type == typeof(Int32) || type == typeof(Int64))
            {
                return ParameterValueType.Integer;
            }
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                return ParameterValueType.Number;
            }

            throw new NotSupportedException();
        }
    }
}
