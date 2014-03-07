using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public static class ConditionModelCollectionExtensions
    {
        public static string GetExpression(this IEnumerable<ConditionModel> conditions)
        {
            var exp = new StringBuilder();
            var first = true;

            // Implicit operator precedence will be handled by the rule engine.
            // So simply concat all same-level condition expressions here.
            foreach(var condition in conditions)
            {
                if (!first)
                {
                    exp.Append(" ").Append(condition.LogicalOperator).Append(" ");
                }

                exp.Append(condition.GetExpression());

                first = false;
            }

            return exp.ToString();
        }
    }
}