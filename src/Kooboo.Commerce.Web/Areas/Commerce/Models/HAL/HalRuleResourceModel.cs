using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class HalRuleResourceModel
    {
        public HalRuleResourceModel()
        { }

        public HalRuleResourceModel(HalRuleResource ruleResource)
        {
            Id = ruleResource.Id;
            RuleId = ruleResource.RuleId;
            ResourceName = ruleResource.ResourceName;
        }

        public void UpdateTo(HalRuleResource ruleResource)
        {
            ruleResource.Id = Id;
            ruleResource.RuleId = RuleId;
            ruleResource.ResourceName = ResourceName;
        }

        public int Id { get; set; }
        public int RuleId { get; set; }
        public string ResourceName { get; set; }
    }
}