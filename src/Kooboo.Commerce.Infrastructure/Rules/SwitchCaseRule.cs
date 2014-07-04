using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class SwitchCaseRule : RuleBase
    {
        public ConditionParameter Parameter { get; set; }

        private Dictionary<object, IList<RuleBase>> _cases;
        public IDictionary<object, IList<RuleBase>> Cases
        {
            get
            {
                if (_cases == null)
                {
                    _cases = new Dictionary<object, IList<RuleBase>>();
                }
                return _cases;
            }
        }

        private List<RuleBase> _default;
        public IList<RuleBase> Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new List<RuleBase>();
                }
                return _default;
            }
        }

        public SwitchCaseRule(ConditionParameter parameter)
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
