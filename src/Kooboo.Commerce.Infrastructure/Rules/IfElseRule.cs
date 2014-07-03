using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class IfElseRule : RuleBase
    {
        private IList<Condition> _conditions;
        private List<RuleBase> _then = new List<RuleBase>();
        private List<RuleBase> _else = new List<RuleBase>();

        public IList<Condition> Conditions
        {
            get
            {
                if (_conditions == null)
                {
                    _conditions = new List<Condition>();
                }

                return _conditions;
            }
            set
            {
                _conditions = value;
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
