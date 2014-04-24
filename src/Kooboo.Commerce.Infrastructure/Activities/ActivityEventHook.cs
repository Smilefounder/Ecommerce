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
    // Activities can only bind to domain events,
    // because listing infrastructure events in the UI is weird.
    // This can also benifit the performance.
    class ActivityEventHook : IHandle<IEvent>
    {
        private IActivityFactory _activityFactory;
        private CommerceInstanceContext _instanceContext;

        public ActivityEventHook(CommerceInstanceContext instanceContext, IActivityFactory activityFactory)
        {
            Require.NotNull(instanceContext, "instanceContext");
            Require.NotNull(activityFactory, "activityFactory");

            _instanceContext = instanceContext;
            _activityFactory = activityFactory;
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
                    RunOrEnqueueActivities(rule.AttachedActivities, rule, @event, activityQueue);
                }
                else
                {
                    if (ruleEngine.CheckCondition(rule.ConditionsExpression, @event))
                    {
                        RunOrEnqueueActivities(rule.ThenActivities, rule, @event, activityQueue);
                    }
                    else
                    {
                        RunOrEnqueueActivities(rule.ElseActivities, rule, @event, activityQueue);
                    }
                }
            }
        }

        private void RunOrEnqueueActivities(IEnumerable<AttachedActivity> attachedActivities, ActivityRule rule, IEvent @event, IRepository<ActivityQueueItem> activityQueue)
        {
            foreach (var attachedActivity in attachedActivities.SortByExecutionOrder())
            {
                if (attachedActivity.IsAsyncExeuctionEnabled)
                {
                    var queueItem = new ActivityQueueItem(attachedActivity, @event);
                    activityQueue.Insert(queueItem);
                }
                else
                {
                    var activity = _activityFactory.FindByName(attachedActivity.ActivityName);
                    if (activity == null)
                        throw new InvalidOperationException("Cannot find activity with name \"" + attachedActivity.ActivityName + "\".");

                    var response = activity.Execute(@event, new ActivityExecutionContext(rule, attachedActivity));
                    if (response == ActivityResult.AbortTransaction)
                    {
                        throw new TransactionAbortException("Activity requested to abort transaction.");
                    }
                }
            }
        }
    }
}
