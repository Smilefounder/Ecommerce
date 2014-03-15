using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    class ActivityExecutor
    {
        private IActivityFactory _activityFactory;
        private IRepository<ActivityRule> _repository;

        public ActivityExecutor(IActivityFactory activityFactory, IRepository<ActivityRule> repository)
        {
            Require.NotNull(activityFactory, "activityFactory");
            Require.NotNull(repository, "repository");

            _activityFactory = activityFactory;
            _repository = repository;
        }

        public void ExecuteActivities(IEvent @event, EventDispatchingContext context)
        {
            var ruleEngine = new RuleEngine();

            foreach (var rule in _repository.Query().ByEvent(@event.GetType()))
            {
                if (rule.Type == RuleType.Always || ruleEngine.CheckCondition(rule.ConditionsExpression, @event))
                {
                    var attachedActivities = rule.AttachedActivities
                                                 .WhereEnabled()
                                                 .SortByExecutionOrder();

                    foreach (var attachedActivity in rule.AttachedActivities)
                    {
                        var activity = _activityFactory.FindByName(attachedActivity.ActivityName);
                        if (activity == null)
                            throw new InvalidOperationException("Cannot find activity with name \"" + attachedActivity.ActivityName + "\".");


                        var awaitAttribute = TryGetAwaitAttribute(activity);

                        if (!EventHandlerUtil.IsTimeToExecute(awaitAttribute, context))
                        {
                            continue;
                        }

                        var response = activity.Execute(@event, new ActivityExecutionContext(rule, attachedActivity));

                        if (response == ActivityResult.SkipSubsequentActivities)
                        {
                            break;
                        }
                        else if (response == ActivityResult.AbortTransaction)
                        {
                            throw new TransactionAbortException("Activity requested to abort transaction.");
                        }
                    }
                }
            }
        }

        private AwaitTransactionCompleteAttribute TryGetAwaitAttribute(IActivity activity)
        {
            return activity.GetType()
                           .GetCustomAttributes(typeof(AwaitTransactionCompleteAttribute), false)
                           .OfType<AwaitTransactionCompleteAttribute>()
                           .FirstOrDefault();
        }
    }
}
