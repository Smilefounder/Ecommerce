using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConfiguredActivityModel
    {
        public string ActivityName { get; set; }

        public string Description { get; set; }

        public string Config { get; set; }

        public bool Async { get; set; }

        public int AsyncDelayDays { get; set; }

        public int AsyncDelayHours { get; set; }

        public int AsyncDelayMinutes { get; set; }

        public int AsyncDelaySeconds { get; set; }

        public static ConfiguredActivityModel FromConfiguredActivity(ConfiguredActivity activity)
        {
            var model = new ConfiguredActivityModel
            {
                ActivityName = activity.ActivityName,
                Description = activity.Description,
                Config = activity.Config,
                Async = activity.Async
            };

            if (activity.Async)
            {
                if (activity.AsyncDelay > 0)
                {
                    var delay = TimeSpan.FromSeconds(activity.AsyncDelay);
                    model.AsyncDelayDays = delay.Days;
                    model.AsyncDelayHours = delay.Hours;
                    model.AsyncDelayMinutes = delay.Minutes;
                    model.AsyncDelaySeconds = delay.Seconds;
                }
            }

            return model;
        }

        public ConfiguredActivity ToConfiguredActivity()
        {
            var activity = new ConfiguredActivity(ActivityName, Description)
            {
                Config = Config,
                Async = Async
            };

            if (activity.Async)
            {
                var delay = new TimeSpan(AsyncDelayDays, AsyncDelayHours, AsyncDelayMinutes, AsyncDelaySeconds);
                activity.AsyncDelay = (int)delay.TotalSeconds;
            }

            return activity;
        }
    }
}