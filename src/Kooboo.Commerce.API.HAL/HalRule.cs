using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class HalRule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ConditionsExpression { get; set; }

        public virtual ICollection<HalRuleResource> Resources { get; set; }
    }
}
