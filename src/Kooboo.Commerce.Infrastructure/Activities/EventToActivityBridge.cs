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
    class EventToActivityBridge : IHandle<IEvent>
    {
        private IActivityProvider _provider;
        private CommerceInstanceContext _instanceContext;

        public EventToActivityBridge(CommerceInstanceContext instanceContext, IActivityProvider provider)
        {
            Require.NotNull(instanceContext, "instanceContext");
            Require.NotNull(provider, "provider");

            _instanceContext = instanceContext;
            _provider = provider;
        }

        public void Handle(IEvent @event)
        {
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
                var descriptor = _provider.GetDescriptorFor(setting.ActivityName);
                if (descriptor == null)
                    throw new InvalidOperationException("Cannot find activity with name \"" + setting.ActivityName + "\".");

                // It's possible that the first version of the activity allows async execution, 
                // and admin configured it to "execute async", 
                // but the second version of the activity doesn't allow async execution.
                // In this case, we need to ignore admin settings, that is, execute it right now
                if (setting.IsAsyncExeuctionEnabled && descriptor.AllowAsyncExecution)
                {
                    var queueItem = new ActivityQueueItem(setting, @event);
                    activityQueue.Insert(queueItem);
                }
                else
                {
                    var activity = (IActivity)EngineContext.Current.Resolve(descriptor.ActivityType);
                    var response = activity.Execute(@event, new ActivityContext(rule, setting, false));
                    // TODO: Consider Quartz.NET's JobExecutionException design
                    if (response == ActivityResult.AbortTransaction)
                    {
                        throw new TransactionAbortException("Activity requested to abort transaction.");
                    }
                }
            }
        }
    }
}
