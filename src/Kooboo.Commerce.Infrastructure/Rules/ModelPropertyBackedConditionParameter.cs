using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ModelPropertyBackedConditionParameter : IConditionParameter
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Type ContextModelType { get; set; }

        public Type ValueType { get; set; }

        public IEnumerable<IComparisonOperator> SupportedOperators { get; set; }

        public PropertyInfo Property { get; set; }

        public ModelPropertyBackedConditionParameter(Type modelType, PropertyInfo property)
        {
            ContextModelType = modelType;
            Name = property.Name;
            DisplayName = Name;
            Property = property;
            ValueType = property.PropertyType;
            TryInitFromConditionParameterAttribute(property);
            SupportedOperators = GetDefaultOperators(ValueType);
        }

        private void TryInitFromConditionParameterAttribute(PropertyInfo property)
        {
            var attr = property.GetCustomAttributes(true)
                               .OfType<ConditionParameterAttribute>()
                               .FirstOrDefault();

            if (attr != null)
            {
                if (!String.IsNullOrEmpty(attr.Name))
                {
                    Name = attr.Name;
                }
                if (!String.IsNullOrEmpty(attr.DisplayName))
                {
                    DisplayName = attr.DisplayName;
                }
            }
        }

        private List<IComparisonOperator> GetDefaultOperators(Type valueType)
        {
            if (valueType == typeof(String))
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotContains,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotContains
                };
            }
            else if (valueType.IsNumber())
            {
                return new List<IComparisonOperator>
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.LessThan,
                    ComparisonOperators.LessThanOrEqual,
                    ComparisonOperators.GreaterThan,
                    ComparisonOperators.GreaterThanOrEqual
                };
            }

            return new List<IComparisonOperator>();
        }

        public object GetValue(object model)
        {
            Require.NotNull(model, "model");
            return Property.GetValue(model, null);
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
    }
}
