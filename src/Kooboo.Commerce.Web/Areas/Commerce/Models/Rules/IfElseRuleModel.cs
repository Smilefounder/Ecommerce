using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class IfElseRuleModel : RuleModelBase
    {
        public IList<Condition> Conditions { get; set; }

        public IList<RuleModelBase> Then { get; set; }

        public IList<RuleModelBase> Else { get; set; }

        public IfElseRuleModel()
            : base("IfElse")
        {
            Conditions = new List<Condition>();
            Then = new List<RuleModelBase>();
            Else = new List<RuleModelBase>();
        }
    }
}