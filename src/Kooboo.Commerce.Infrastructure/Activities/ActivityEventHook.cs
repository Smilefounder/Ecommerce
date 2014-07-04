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

namespace Kooboo.Commerce.Activities
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

        private void Execute(IEvent @event, Type eventType, CommerceInstance commerceInstance)
        {
            var database = commerceInstance.Database;
            var ruleEngine = new ConditionEvaluator();

            var activityQueue = database.GetRepository<ActivityQueueItem>();
            var ruleManager = RuleManager.GetManager(commerceInstance.Name);
            var rules = ruleManager.GetRules(eventType.Name);

            var activities = new List<ConfiguredActivity>();

            foreach (var rule in rules)
            {
                activities.AddRange(rule.Execute(@event));
            }

            RunOrEnqueueActivities(activities, @event, activityQueue);
        }

        private void RunOrEnqueueActivities(IEnumerable<ConfiguredActivity> settings, IEvent @event, IRepository<ActivityQueueItem> activityQueue)
        {
            foreach (var setting in settings)
            {
                var activity = _provider.FindByName(setting.ActivityName);
                // If the activity is missing, then ignore it
                if (activity == null)
                {
                    continue;
                }

                // It's possible that the first version of the activity allows async execution, 
                // and admin configured it to "execute async", 
                // but the second version of the activity doesn't allow async execution.
                // In this case, we need to ignore admin settings, that is, execute it right now
                if (setting.Async && activity.AllowAsyncExecution)
                {
                    var queueItem = new ActivityQueueItem(@event, setting);
                    activityQueue.Insert(queueItem);
                }
                else
                {
                    object parameters = null;
                    if (activity.ConfigModelType != null)
                    {
                        parameters = setting.LoadConfigModel(activity.ConfigModelType);
                    }

                    activity.Execute(@event, new ActivityContext(parameters, false));
                }
            }
        }
    }
}
