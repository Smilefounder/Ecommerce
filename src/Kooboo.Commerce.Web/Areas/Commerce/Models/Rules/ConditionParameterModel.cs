using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConditionParameterModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ParameterValueType ValueType { get; set; }

        public IList<ComparisonOperatorModel> SupportedOperators { get; set; }

        public ConditionParameterModel()
        {
            SupportedOperators = new List<ComparisonOperatorModel>();
        }

        public ConditionParameterModel(IConditionParameter param) : this()
        {
            Name = param.Name;
            DisplayName = param.DisplayName;
            ValueType = param.ValueType;

            foreach (var @operator in param.SupportedOperators)
            {
                SupportedOperators.Add(new ComparisonOperatorModel(@operator));
            }
        }
    }

    public class ComparisonOperatorModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ComparisonOperatorModel() { }

        public ComparisonOperatorModel(IComparisonOperator @operator)
        {
            Name = @operator.Name;
            DisplayName = @operator.DisplayName;
        }
    }
}