using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions
{
    public class ConditionModel
    {
        public ConditionType Type { get; set; }

        public string Expression { get; set; }

        public List<ComparisonGroup> Groups { get; set; }

        public ConditionModel()
        {
            Groups = new List<ComparisonGroup>();
        }

        public string GetExpression()
        {
            if (Groups.Count == 0)
            {
                return null;
            }

            if (Groups.Count == 1)
            {
                return Groups[0].GetExpression();
            }

            var exp = new StringBuilder();
            var first = true;

            foreach (var group in Groups)
            {
                if (group.Comparisons.Count > 0)
                {
                    if (!first)
                    {
                        exp.Append(" AND ");
                    }
                    exp.Append("(" + group.GetExpression() + ")");

                    first = false;
                }
            }

            return exp.ToString();
        }
    }

    public class ComparisonGroup
    {
        public List<ComparisonModel> Comparisons { get; set; }

        public ComparisonGroup()
        {
            Comparisons = new List<ComparisonModel>();
        }

        public string GetExpression()
        {
            return String.Join(" OR ", Comparisons.Select(c => c.GetExpression()));
        }
    }
}