using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class IfElseRule : Rule
    {
        private IList<Condition> _conditions;
        private List<Rule> _then = new List<Rule>();
        private List<Rule> _else = new List<Rule>();

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

        public IList<Rule> Then
        {
            get
            {
                return _then;
            }
        }

        public IList<Rule> Else
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
