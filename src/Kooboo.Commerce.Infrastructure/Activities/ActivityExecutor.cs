using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    class ActivityExecutor
    {
        private IActivityFactory _activityFactory;
        private IRepository<ActivityBinding> _repository;

        public ActivityExecutor(IActivityFactory activityFactory, IRepository<ActivityBinding> repository)
        {
            Require.NotNull(activityFactory, "activityFactory");
            Require.NotNull(repository, "repository");

            _activityFactory = activityFactory;
            _repository = repository;
        }

        public void ExecuteActivities(IEvent @event, EventDispatchingContext context)
        {
            foreach (var binding in GetBindings(@event))
            {
                var activity = _activityFactory.FindByName(binding.ActivityName);
                if (activity == null)
                    throw new InvalidOperationException("Cannot find activity with name \"" + binding.ActivityName + "\".");

                var awaitAttribute = TryGetAwaitAttribute(activity);

                if (!EventHandlerUtil.IsTimeToExecute(awaitAttribute, context))
                {
                    continue;
                }

                var response = activity.Execute(@event, binding);

                if (response == ActivityResponse.SkipSubsequentActivities)
                {
                    break;
                }
                else if (response == ActivityResponse.AbortTransaction)
                {
                    throw new TransactionAbortException("Activity requested to abort transaction.");
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

        private IEnumerable<ActivityBinding> GetBindings(IEvent @event)
        {
            return _repository.Query()
                              .WhereBoundToEvent(@event.GetType())
                              .Where(x => x.IsEnabled)
                              .OrderByExecutionOrder()
                              .ToList();
        }
    }
}
