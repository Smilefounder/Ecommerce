using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Represents a condition expression parameter can be constructed dynamcially.
    /// </summary>
    public class DynamicParameter : IParameter
    {
        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public Type ModelType { get; private set; }

        public Type ValueType { get; private set; }

        public IEnumerable<IComparisonOperator> SupportedOperators { get; private set; }

        public PropertyInfo PropertyPath { get; private set; }

        private DynamicParameter() { }

        public object GetValue(object model)
        {
            Require.NotNull(model, "model");
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

        /// <summary>
        /// Try construct the condition expression parameter from a model property.
        /// </summary>
        public static DynamicParameter TryCreateFrom(PropertyInfo property)
        {
            Require.NotNull(property, "property");

            var attribute = property.GetCustomAttributes(typeof(ParameterAttribute), true)
                                    .OfType<ParameterAttribute>()
                                    .FirstOrDefault();

            if (attribute != null)
            {
                return CreateFrom(property, attribute);
            }

            return null;
        }

        static DynamicParameter CreateFrom(PropertyInfo property, ParameterAttribute attribute)
        {
            var param = new DynamicParameter
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
