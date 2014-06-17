using Kooboo.CMS.Common.Runtime;
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

        public string ValueType { get; set; }

        public bool IsNumberValue { get; set; }

        public IList<ComparisonOperatorModel> SupportedOperators { get; set; }

        public IList<ParameterValueItem> Values { get; set; }

        public ConditionParameterModel()
        {
            Values = new List<ParameterValueItem>();
            SupportedOperators = new List<ComparisonOperatorModel>();
        }

        public ConditionParameterModel(ConditionParameter param) : this()
        {
            Name = param.Name;
            DisplayName = param.Name;
            ValueType = param.ValueType.FullName;
            IsNumberValue = param.ValueType.IsNumber();

            foreach (var @operator in param.SupportedOperators)
            {
                SupportedOperators.Add(new ComparisonOperatorModel(@operator));
            }

            if (param.ValueSource != null)
            {
                Values = param.ValueSource.GetValues(param).ToList();
            }
        }
    }

    public class ComparisonOperatorModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Alias { get; set; }

        public ComparisonOperatorModel() { }

        public ComparisonOperatorModel(IComparisonOperator @operator)
        {
            Name = @operator.Name;
            DisplayName = @operator.DisplayName;
            Alias = @operator.Alias;
        }
    }
}