using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public static class ConditionModelCollectionExtensions
    {
        public static string GetExpression(this IList<ConditionModel> conditions)
        {
            var exp = new StringBuilder();

            for (var i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];

                if (i > 0)
                {
                    if (condition.LogicalOperator == ConditionLogicalOperator.ThenAND || condition.LogicalOperator == ConditionLogicalOperator.ThenOR)
                    {
                        exp.Insert(0, "(");
                        exp.Append(")");
                    }

                    if (condition.LogicalOperator == ConditionLogicalOperator.AND || condition.LogicalOperator == ConditionLogicalOperator.ThenAND)
                    {
                        exp.Append(" AND ");
                    }
                    else if (condition.LogicalOperator == ConditionLogicalOperator.OR || condition.LogicalOperator == ConditionLogicalOperator.ThenOR)
                    {
                        exp.Append(" OR ");
                    }
                }

                exp.Append(condition.GetExpression());
            }

            return exp.ToString();
        }
    }
}