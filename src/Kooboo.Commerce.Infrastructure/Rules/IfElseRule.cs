using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class IfElseRule : RuleBase
    {
        private List<Condition> _conditions = new List<Condition>();
        private List<RuleBase> _then = new List<RuleBase>();
        private List<RuleBase> _else = new List<RuleBase>();

        public IList<Condition> Conditions
        {
            get
            {
                return _conditions;
            }
        }

        public IList<RuleBase> Then
        {
            get
            {
                return _then;
            }
        }

        public IList<RuleBase> Else
        {
            get
            {
                return _else;
            }
        }

        public override IEnumerable<ConfiguredActivity> Execute(object dataContext)
        {
            var evaluator = new ConditionEvaluator();
            var success = evaluator.Evaluate(Conditions, dataContext);

            if (success)
            {
                return Further(Then, dataContext);
            }
            else
            {
                return Further(Else, dataContext);
            }
        }
    }
}
