using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Kooboo.Commerce.Activities
{
    public class AttachedActivityInfo
    {
        public virtual int Id { get; set; }

        [Required, StringLength(100)]
        public virtual string Description { get; set; }

        [Required, StringLength(100)]
        public virtual string ActivityName { get; set; }

        private string ActivityConfig { get; set; }

        public virtual T LoadActivityConfig<T>()
            where T : class
        {
            return LoadActivityConfig(typeof(T)) as T;
        }

        public virtual object LoadActivityConfig(Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(ActivityConfig))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(ActivityConfig, configModelType);
        }

        public virtual void UpdateActivityConfig(object configModel)
        {
            if (configModel == null)
            {
                ActivityConfig = null;
            }
            else
            {
                ActivityConfig = JsonConvert.SerializeObject(configModel);
            }
        }

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

        public AttachedActivityInfo(ActivityRule rule, RuleBranch branch, string description, string activityName)
        {
            Require.NotNullOrEmpty(description, "description");
            Require.NotNullOrEmpty(activityName, "activityName");

            Rule = rule;
            RuleBranch = branch;
            Description = description;
            ActivityName = activityName;
            CreatedAtUtc = DateTime.UtcNow;
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

        #region Entity Configuration

        class AttachedActivityMap : EntityTypeConfiguration<AttachedActivityInfo>
        {
            public AttachedActivityMap()
            {
                Property(c => c.ActivityConfig);
            }
        }

        #endregion
    }
}
