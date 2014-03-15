using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class DynamicConditionParameter : IConditionParameter
    {
        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public Type ModelType { get; private set; }

        public Type ValueType { get; private set; }

        public IEnumerable<IComparisonOperator> SupportedOperators { get; private set; }

        public PropertyInfo PropertyPath { get; private set; }

        private DynamicConditionParameter() { }

        public object GetValue(object model)
        {
            return PropertyPath.GetValue(model, null);
        }

        public object ParseValue(string value)
        {
            if (ValueType.IsEnum)
            {
                return Enum.Parse(ValueType, value, true);
            }

            return Convert.ChangeType(value, ValueType);
        }

        public override string ToString()
        {
            return Name;
        }

        public static DynamicConditionParameter TryCreateFrom(PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes(typeof(ConditionParameterAttribute), true)
                                    .OfType<ConditionParameterAttribute>()
                                    .FirstOrDefault();

            if (attribute != null)
            {
                return CreateFrom(property, attribute);
            }

            return null;
        }

        static DynamicConditionParameter CreateFrom(PropertyInfo property, ConditionParameterAttribute attribute)
        {
            var param = new DynamicConditionParameter
            {
                Name = String.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name,
                DisplayName = String.IsNullOrEmpty(attribute.DisplayName) ? property.Name : attribute.DisplayName,
                ModelType = property.ReflectedType,
                PropertyPath = property,
                ValueType = property.PropertyType
            };

            if (param.ValueType == typeof(String))
            {
                param.SupportedOperators = new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotContains
                };
            }
            else
            {
                param.SupportedOperators = new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.LessThan,
                    ComparisonOperators.LessThanOrEqual,
                    ComparisonOperators.GreaterThan,
                    ComparisonOperators.GreaterThanOrEqual
                };
            }

            return param;
        }
    }
}
