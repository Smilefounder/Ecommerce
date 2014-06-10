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
            if (!(@event is DomainEvent))
            {
                return;
            }

            var commerceInstance = _instanceContext.CurrentInstance;

            // Activities must be executed within commerce instance context
            if (commerceInstance == null)
            {
                return;
            }

            var database = commerceInstance.Database;
            var ruleEngine = EngineContext.Current.Resolve<RuleEngine>();

            var activityQueue = database.GetRepository<ActivityQueueItem>();
            var rules = database.GetRepository<ActivityRule>()
                                .Query()
                                .ByEvent(@event.GetType())
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
                    if (ruleEngine.CheckCondition(rule.ConditionsExpression, @event))
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
                    activity.Execute(@event, new ActivityContext(rule, setting, false));
                }
            }
        }
    }
}
