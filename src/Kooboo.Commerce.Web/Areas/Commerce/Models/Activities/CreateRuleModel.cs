using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class CreateRuleModel
    {
        public string EventType { get; set; }

        public List<ConditionModel> Conditions { get; set; }

        public CreateRuleModel()
        {
            Conditions = new List<ConditionModel>();
        }
    }
}