using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class UpdateConditionsModel
    {
        public int RuleId { get; set; }

        public List<Condition> Conditions { get; set; }

        public UpdateConditionsModel()
        {
            Conditions = new List<Condition>();
        }
    }
}