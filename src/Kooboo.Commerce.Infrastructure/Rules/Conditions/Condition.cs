using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class Condition
    {
        public string Expression { get; set; }

        public ConditionType Type { get; set; }

        public Condition() { }

        public Condition(string expression, ConditionType type)
        {
            Expression = expression;
            Type = type;
        }
    }

    public enum ConditionType
    {
        Include = 0,
        Exclude = 1
    }
}
