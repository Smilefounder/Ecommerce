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

        public IList<ParameterValue> Values { get; set; }

        public ConditionParameterModel()
        {
            Values = new List<ParameterValue>();
            SupportedOperators = new List<ComparisonOperatorModel>();
        }

        public ConditionParameterModel(IParameter param) : this()
        {
            Name = param.Name;
            DisplayName = param.DisplayName;
            ValueType = param.ValueType.FullName;
            IsNumberValue = param.ValueType.IsNumber();

            foreach (var @operator in param.SupportedOperators)
            {
                SupportedOperators.Add(new ComparisonOperatorModel(@operator));
            }

            var valueSourceFactory = EngineContext.Current.Resolve<IParameterValueDataSourceProvider>();
            var valueSources = valueSourceFactory.GetDataSources(param.Name).ToList();

            if (valueSources.Count > 0)
            {
                Values = valueSources.SelectMany(x => x.GetValues(param)).ToList();
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