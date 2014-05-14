using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityEditorModel
    {
        public int AttachedActivityInfoId { get; set; }

        public string Description { get; set; }

        public ActivityDescriptorModel ActivityDescriptor { get; set; }

        public bool IsEnabled { get; set; }

        public int Priority { get; set; }

        public bool EnableAsyncExecution { get; set; }

        public int DelayDays { get; set; }

        public int DelayHours { get; set; }

        public int DelayMinutes { get; set; }

        public int DelaySeconds { get; set; }

        public int RuleId { get; set; }

        public RuleBranch RuleBranch { get; set; }

        public ActivityEditorModel()
        {
            IsEnabled = true;
        }
    }
}