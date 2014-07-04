using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions.Operators;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions
{
    public class RuleParameterModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ValueType { get; set; }

        public bool IsNumberValue { get; set; }

        public IList<ComparisonOperatorModel> SupportedOperators { get; set; }

        public IList<SelectListItem> Values { get; set; }

        public RuleParameterModel()
        {
            Values = new List<SelectListItem>();
            SupportedOperators = new List<ComparisonOperatorModel>();
        }

        public RuleParameterModel(RuleParameter param)
            : this()
        {
            Name = param.Name;
            DisplayName = param.Name;
            ValueType = param.ValueType.FullName;
            IsNumberValue = param.ValueType.IsNumericType();

            foreach (var @operator in param.SupportedOperators)
            {
                SupportedOperators.Add(new ComparisonOperatorModel(@operator));
            }

            if (param.ValueSource != null)
            {
                Values = param.ValueSource.GetValues(param).Select(x => new SelectListItem
                {
                    Text = x.Key,
                    Value = x.Value
                })
                .ToList();
            }
        }
    }

    public class ComparisonOperatorModel
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string DisplayName { get; set; }

        public ComparisonOperatorModel() { }

        public ComparisonOperatorModel(IComparisonOperator @operator)
        {
            Name = @operator.Name;
            Alias = @operator.Alias;
            DisplayName = String.IsNullOrWhiteSpace(@operator.Alias) ? Name : Alias;
        }
    }
}