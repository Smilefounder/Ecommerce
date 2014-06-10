using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Globalization;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class AttachedActivityModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ActivityName { get; set; }

        public string ActivityDisplayName { get; set; }

        public bool ActivityAllowAsyncExecution { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsActivityMissing { get; set; }

        public int Priority { get; set; }

        [Display(Name = "Enable Async Execution")]
        public bool IsAsyncExecutionEnabled { get; set; }

        public int DelayDays { get; set; }

        public int DelayHours { get; set; }

        public int DelayMinutes { get; set; }

        public int DelaySeconds { get; set; }

        public string DelayHint { get; set; }

        public string ConfigUrl { get; set; }

        public int RuleId { get; set; }

        public RuleBranch RuleBranch { get; set; }

        public AttachedActivityModel()
        {
            IsEnabled = true;
        }

        public AttachedActivityModel(AttachedActivityInfo attachedActivityInfo)
        {
            Id = attachedActivityInfo.Id;
            RuleId = attachedActivityInfo.Rule.Id;
            Description = attachedActivityInfo.Description;
            ActivityName = attachedActivityInfo.ActivityName;

            var activity = EngineContext.Current.Resolve<IActivityProvider>().FindByName(attachedActivityInfo.ActivityName);
            if (activity != null)
            {
                ActivityDisplayName = activity.DisplayName;
            }
            else
            {
                IsActivityMissing = true;
            }

            IsEnabled = attachedActivityInfo.IsEnabled;
            Priority = attachedActivityInfo.Priority;
            RuleBranch = attachedActivityInfo.RuleBranch;
            IsAsyncExecutionEnabled = attachedActivityInfo.IsAsyncExeuctionEnabled;

            if (attachedActivityInfo.AsyncExecutionDelay > 0)
            {
                var delay = TimeSpan.FromSeconds(attachedActivityInfo.AsyncExecutionDelay);
                DelayDays = delay.Days;
                DelayHours = delay.Hours;
                DelayMinutes = delay.Minutes;
                DelaySeconds = delay.Seconds;
            }

            if (IsAsyncExecutionEnabled)
            {
                if (attachedActivityInfo.AsyncExecutionDelay > 0)
                {
                    DelayHint = String.Format("Will execute in {0} when the event occurs".Localize(), TimeSpan.FromSeconds(attachedActivityInfo.AsyncExecutionDelay).Humanize());
                }
                else
                {
                    DelayHint = "Will execute asynchrously".Localize();
                }
            }
        }
    }
}