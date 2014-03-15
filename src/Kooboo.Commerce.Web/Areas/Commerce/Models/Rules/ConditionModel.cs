using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConditionModel
    {
        /// <summary>
        /// The logical operator to connect this condition with last condition.
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; }

        public string ParamName { get; set; }

        public string ComparisonOperator { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }

        public bool IsGroup { get; set; }

        public IList<ConditionModel> Conditions { get; set; }

        public ConditionModel()
        {
            Conditions = new List<ConditionModel>();
        }

        public void AddConditions(IEnumerable<ConditionModel> conditions)
        {
            foreach (var condition in conditions)
            {
                Conditions.Add(condition);
            }
        }

        public string GetExpression()
        {
            var exp = new StringBuilder();

            if (IsGroup)
            {
                exp.Append("(");
                exp.Append(Conditions.GetExpression());
                exp.Append(")");
            }
            else
            {
                exp.Append(ParamName).Append(" ").Append(ComparisonOperators.TryGetOperatorShortcut(ComparisonOperator)).Append(" ");
                if (ValueType == typeof(String).FullName)
                {
                    exp.Append("\"").Append(Value).Append("\"");
                }
                else
                {
                    exp.Append(Value);
                }
            }

            return exp.ToString();
        }

        public override string ToString()
        {
            return LogicalOperator + " " + ParamName + " " + ComparisonOperator + " " + Value;
        }
    }
}