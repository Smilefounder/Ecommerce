using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public enum ConditionLogicalOperator
    {
        AND = 0,
        OR = 1,
        ThenAND = 2,
        ThenOR = 3
    }

    public class ConditionModel
    {
        /// <summary>
        /// The logical operator to connect last condition with this condition .
        /// </summary>
        public ConditionLogicalOperator LogicalOperator { get; set; }

        public string ParamName { get; set; }

        public string ComparisonOperator { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }

        public bool IsNumberValue { get; set; }

        public string GetExpression()
        {
            var value = Value;
            if (!IsNumberValue)
            {
                value = "\"" + value + "\"";
            }

            return ParamName + " " + ComparisonOperator + " " + value;
        }

        public override string ToString()
        {
            return GetExpression();
        }
    }
}