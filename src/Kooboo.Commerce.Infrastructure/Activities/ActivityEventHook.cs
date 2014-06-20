using Kooboo.Commerce.Events;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Rules;

namespace Kooboo.Commerce.Activities
{
    class ActivityEventHook : IHandle<IEvent>
    {
        private IActivityProvider _provider;
        private CommerceInstanceContext _instanceContext;

        public ActivityEventHook(CommerceInstanceContext instanceContext, IActivityProvider provider)
        {
            Require.NotNull(instanceContext, "instanceContext");
            Require.NotNull(provider, "provider");

            _instanceContext = instanceContext;
            _provider = provider;
        }

        public void Handle(IEvent @event)
        {
            // Activities can only bind to domain events
            if (!(@event is BusinessEvent))
            {
                return;
            }

            var commerceInstance = _instanceContext.CurrentInstance;

            // Activities must be executed within commerce instance context
            if (commerceInstance == null)
            {
                return;
            }

            Execute(@event, @event.GetType(), commerceInstance);

            // Execute activities bound to base event types
            var baseType = @event.GetType().BaseType;
            while (baseType != null && typeof(IBusinessEvent).IsAssignableFrom(baseType))
            {
                Execute(@event, baseType, commerceInstance);
                baseType = baseType.GetType().BaseType;
            }
        }

        private void Execute(IEvent @event, Type eventType, CommerceInstance commerceInstance)
        {
            var database = commerceInstance.Database;
            var ruleEngine = new RuleEngine();

            var activityQueue = database.GetRepository<ActivityQueueItem>();
            var rules = database.GetRepository<ActivityRule>()
                                .Query()
                                .ByEvent(eventType)
                                .OrderBy(x => x.Id)
                                .ToList();

            foreach (var rule in rules)
            {
                if (rule.Type == RuleType.Always)
                {
                    RunOrEnqueueActivities(rule.AttachedActivityInfos, rule, @event, activityQueue);
                }
                else
                {
                    if (ruleEngine.CheckConditions(rule.Conditions, @event))
                    {
                        RunOrEnqueueActivities(rule.ThenActivityInfos, rule, @event, activityQueue);
                    }
                    else
                    {
                        RunOrEnqueueActivities(rule.ElseActivityInfos, rule, @event, activityQueue);
                    }
                }
            }
        }

        private void RunOrEnqueueActivities(IEnumerable<AttachedActivityInfo> settings, ActivityRule rule, IEvent @event, IRepository<ActivityQueueItem> activityQueue)
        {
            foreach (var setting in settings.OrderByExecutionOrder())
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
                if (setting.IsAsyncExeuctionEnabled && activity.AllowAsyncExecution)
                {
                    var queueItem = new ActivityQueueItem(setting, @event);
                    activityQueue.Insert(queueItem);
                }
                else
                {
                    ActivityParameters parameters = null;
                    if (activity.ParametersType != null)
                    {
                        parameters = ActivityParameters.Create(activity.ParametersType, setting.GetParameters());
                    }

                    activity.Execute(@event, new ActivityContext(parameters, false));
                }
            }
        }
    }
}
