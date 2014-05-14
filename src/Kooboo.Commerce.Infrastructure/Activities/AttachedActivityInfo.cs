using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
using Newtonsoft.Json;

namespace Kooboo.Commerce.Activities
{
    public class AttachedActivityInfo
    {
        public virtual int Id { get; set; }

        [Required, StringLength(100)]
        public virtual string Description { get; set; }

        public virtual string ActivityName { get; set; }

        // TODO: Make private
        public virtual string ActivityConfig { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual int Priority { get; set; }

        /// <summary>
        /// Get or sets if this activity need to execute asynchrously.
        /// </summary>
        public virtual bool IsAsyncExeuctionEnabled { get; set; }

        /// <summary>
        /// Get or sets the delay (in seconds) from the time the event occurs.
        /// </summary>
        public virtual int AsyncExecutionDelay { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual ActivityRule Rule { get; set; }

        public virtual RuleBranch RuleBranch { get; set; }

        protected AttachedActivityInfo()
        {
        }

        public AttachedActivityInfo(ActivityRule rule, RuleBranch branch, string description, string activityName, object config = null)
        {
            Require.NotNullOrEmpty(description, "description");
            Require.NotNullOrEmpty(activityName, "activityName");

            Rule = rule;
            RuleBranch = branch;
            Description = description;
            ActivityName = activityName;
            CreatedAtUtc = DateTime.UtcNow;

            if (config != null)
            {
                SetActivityConfig(config);
            }
        }

        public T GetActivityConfig<T>()
        {
            if (String.IsNullOrEmpty(ActivityConfig))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(ActivityConfig);
        }

        public void SetActivityConfig(object config)
        {
            if (config == null)
            {
                ActivityConfig = null;
            }
            else
            {
                ActivityConfig = JsonConvert.SerializeObject(config);
            }
        }

        public DateTime CalculateExecutionTime(DateTime eventTimeUtc)
        {
            if (!IsAsyncExeuctionEnabled)
            {
                return eventTimeUtc;
            }

            return eventTimeUtc.AddSeconds(AsyncExecutionDelay);
        }

        public void EnableAsyncExecution(int delay)
        {
            IsAsyncExeuctionEnabled = true;
            AsyncExecutionDelay = delay;
        }

        public void UpdateAsyncExecutionDelay(int newDelay)
        {
            AsyncExecutionDelay = newDelay;
        }

        public void DisableAsyncExecution()
        {
            IsAsyncExeuctionEnabled = false;
        }
    }
}
