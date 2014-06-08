using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityRuleBranchModel
    {
        public int RuleId { get; set; }

        public RuleBranch Branch { get; set; }

        public List<AttachedActivityModel> AttachedActivities { get; set; }

        public ActivityRuleBranchModel()
        {
            AttachedActivities = new List<AttachedActivityModel>();
        }
    }
}