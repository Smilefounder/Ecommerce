using Kooboo.Commerce.Events;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Activities.Scheduling;

namespace Kooboo.Commerce.Rules.Activities
{
    class ActivityEventHook : IHandle<IEvent>
    {
        private IActivityProvider _provider;

        public ActivityEventHook(IActivityProvider provider)
        {
            Require.NotNull(provider, "provider");
            _provider = provider;
        }

        public void Handle(IEvent @event)
        {
            if (!ActivityEventManager.Instance.IsEventRegistered(@event.GetType()))
            {
                return;
            }

            var instance = CommerceInstance.Current;
            // Activities must be executed within commerce instance context
            if (instance == null)
            {
                return;
            }

            Execute(@event, @event.GetType(), instance);

            // Execute activities bound to base event types
            var baseType = @event.GetType().BaseType;
            while (baseType != null && typeof(IEvent).IsAssignableFrom(baseType))
            {
                Execute(@event, baseType, instance);
                baseType = baseType.GetType().BaseType;
            }
        }

        private void Execute(IEvent @event, Type eventType, CommerceInstance instance)
        {
            var ruleManager = RuleManager.GetManager(instance.Name);

            // Execute rules and get the activities to execute
            var activities = new List<ConfiguredActivity>();
            foreach (var rule in ruleManager.GetRules(eventType.Name))
            {
                activities.AddRange(rule.Execute(@event));
            }

            // Schedule or execute activities
            ScheduleActivities(activities.Where(x => x.Async), @event, instance.Database);
            ExecuteActivities(activities.Where(x => !x.Async), @event);
        }

        private void ScheduleActivities(IEnumerable<ConfiguredActivity> configuredActivities, IEvent @event, ICommerceDatabase database)
        {
            var repository = database.Repository<ScheduledActivity>();
            foreach (var each in configuredActivities)
            {
                repository.Insert(new ScheduledActivity(@event, each));
            }
            database.SaveChanges();
        }

        private void ExecuteActivities(IEnumerable<ConfiguredActivity> configuredActivities, IEvent @event)
        {
            foreach (var configuredActivity in configuredActivities)
            {
                var activity = _provider.FindByName(configuredActivity.ActivityName);
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
