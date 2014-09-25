using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Activities.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class EventSlot
    {
        public Type EventType { get; private set; }

        public string ShortName { get; private set; }

        public EventSlot(Type eventType)
            : this(eventType, null)
        {
        }

        public EventSlot(Type eventType, string shortName)
        {
            Require.NotNull(eventType, "eventType");
            EventType = eventType;
            ShortName = shortName;
        }

        public void Initialize()
        {
            Event.Listen(EventType, Handle);
        }

        private void Handle(IEvent @event, CommerceInstance commerceInstance)
        {
            // TODO: Need to be passed in?
            var instance = CommerceInstance.Current;

            var rules = RuleManager.GetManager(instance.Name).GetRules(@event.GetType().Name);
            var activities = new List<ConfiguredActivity>();
            foreach (var rule in rules)
            {
                activities.AddRange(rule.Execute(@event));
            }

            ScheduleActivities(activities.Where(it => it.Async), @event, instance);
            ExecuteActivities(activities.Where(it => !it.Async), @event);
        }

        private void ScheduleActivities(IEnumerable<ConfiguredActivity> activities, IEvent @event, CommerceInstance instance)
        {
            var repository = instance.Database.Repository<ScheduledActivity>();

            foreach (var activity in activities)
            {
                repository.Insert(new ScheduledActivity(@event, activity));
            }
        }

        private void ExecuteActivities(IEnumerable<ConfiguredActivity> configuredActivities, IEvent @event)
        {
            var activityProvider = EngineContext.Current.Resolve<IActivityProvider>();

            foreach (var configuredActivity in configuredActivities)
            {
                var activity = activityProvider.FindByName(configuredActivity.ActivityName);
                // If the activity is missing, then ignore it
                if (activity == null)
                {
                    continue;
                }

                object config = null;
                if (activity.ConfigType != null)
                {
                    config = configuredActivity.LoadConfigModel(activity.ConfigType);
                }

                activity.Execute(@event, new ActivityContext(config, false));
            }
        }
    }
}
