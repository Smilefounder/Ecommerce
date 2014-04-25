using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class HalRuleResource
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public string ResourceName { get; set; }

        public virtual HalRule Rule { get; set; }

    }
}
