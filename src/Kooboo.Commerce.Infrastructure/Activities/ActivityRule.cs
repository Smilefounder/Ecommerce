using Kooboo.Commerce.Activities.Events;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Rules;
using Kooboo.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityRule
    {
        protected ActivityRule() { }

        public ActivityRule(Type eventType, RuleType type)
        {
            EventType = eventType.AssemblyQualifiedNameWithoutVersion();
            Type = type;
            CreatedAtUtc = DateTime.UtcNow;
            AttachedActivityInfos = new List<AttachedActivityInfo>();
        }

        public int Id { get; set; }

        [Required, StringLength(500)]
        public string EventType { get; set; }

        public RuleType Type { get; set; }

        [StringLength(4000)]
        private string ConditionsJson { get; set; }

        private List<Condition> _conditions;

        [NotMapped]
        public IEnumerable<Condition> Conditions
        {
            get
            {
                if (_conditions == null)
                {
                    if (String.IsNullOrWhiteSpace(ConditionsJson))
                    {
                        _conditions = new List<Condition>();
                    }
                    else
                    {
                        _conditions = JsonConvert.DeserializeObject<List<Condition>>(ConditionsJson);
                    }
                }

                return _conditions;
            }
            set
            {
                if (value == null)
                {
                    _conditions = new List<Condition>();
                    ConditionsJson = null;
                }
                else
                {
                    _conditions = value.ToList();
                    ConditionsJson = JsonConvert.SerializeObject(_conditions);
                }
            }
        }

        public virtual ICollection<AttachedActivityInfo> AttachedActivityInfos { get; protected set; }

        [NotMapped]
        public virtual IEnumerable<AttachedActivityInfo> ThenActivityInfos
        {
            get
            {
                return AttachedActivityInfos.Where(x => x.RuleBranch == RuleBranch.Then).ToList();
            }
        }

        [NotMapped]
        public virtual IEnumerable<AttachedActivityInfo> ElseActivityInfos
        {
            get
            {
                return AttachedActivityInfos.Where(x => x.RuleBranch == RuleBranch.Else).ToList();
            }
        }

        public DateTime CreatedAtUtc { get; set; }

        public AttachedActivityInfo AttachActivity(RuleBranch branch, string description, string activityName)
        {
            var attachedActivity = new AttachedActivityInfo(this, branch, description, activityName);
            AttachedActivityInfos.Add(attachedActivity);

            Event.Raise(new ActivityAttached(this, attachedActivity));

            return attachedActivity;
        }

        public bool DetachActivity(int attachedActivityInfoId)
        {
            var attachedActivity = AttachedActivityInfos.Find(attachedActivityInfoId);
            if (attachedActivity != null)
            {
                return DetachActivity(attachedActivity);
            }

            return false;
        }

        public bool DetachActivity(AttachedActivityInfo attachedActivityInfo)
        {
            Require.NotNull(attachedActivityInfo, "attachedActivityInfo");

            var detached = AttachedActivityInfos.Remove(attachedActivityInfo);
            if (detached)
            {
                Event.Raise(new ActivityDetached(this, attachedActivityInfo));
            }

            return detached;
        }

        public void DetachAllActivities()
        {
            foreach (var activity in AttachedActivityInfos.ToList())
            {
                DetachActivity(activity);
            }
        }

        #region Entity Configuration

        class ActivityRuleMap : EntityTypeConfiguration<ActivityRule>
        {
            public ActivityRuleMap()
            {
                Property(c => c.ConditionsJson);
                HasMany(x => x.AttachedActivityInfos)
                    .WithRequired(x => x.Rule)
                    .Map(x => x.MapKey("ActivityRule_Id"));
            }
        }

        #endregion
    }
}
