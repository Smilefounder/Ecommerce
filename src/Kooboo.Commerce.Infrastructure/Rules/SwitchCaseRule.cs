using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class SwitchCaseRule : Rule
    {
        public RuleParameter Parameter { get; set; }

        private Dictionary<object, IList<Rule>> _cases;
        public IDictionary<object, IList<Rule>> Cases
        {
            get
            {
                if (_cases == null)
                {
                    _cases = new Dictionary<object, IList<Rule>>();
                }
                return _cases;
            }
        }

        private List<Rule> _default;
        public IList<Rule> Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new List<Rule>();
                }
                return _default;
            }
        }

        public SwitchCaseRule(RuleParameter parameter)
        {
            Parameter = parameter;
        }

        public override IEnumerable<ConfiguredActivity> Execute(object dataContext)
        {
            var paramValue = Parameter.ResolveValue(dataContext);

            if (paramValue != null && Cases.ContainsKey(paramValue))
            {
                return Further(Cases[paramValue], dataContext);
            }

            return Further(Default, dataContext);
        }
    }
}
