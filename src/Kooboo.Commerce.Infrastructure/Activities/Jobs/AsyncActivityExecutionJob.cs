using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities.Jobs
{
    public class AsyncActivityExecutionJob : IJob
    {
        public bool InProgress { get; private set; }

        public void Error(Exception e)
        {
            // TODO: Logging

            InProgress = false;
        }

        public void Execute(object executionState)
        {
            // Prevent the job execution overlapping
            if (InProgress)
            {
                return;
            }

            InProgress = true;

            var engine = EngineContext.Current;
            var instanceManager = engine.Resolve<ICommerceInstanceManager>();
            var activityFactory = engine.Resolve<IActivityFactory>();
            var now = DateTime.UtcNow;

            foreach (var metadata in instanceManager.GetAllInstanceMetadatas())
            {
                var instance = instanceManager.OpenInstance(metadata.Name);

                // Begin a scope so the activities are able to resolve dependencies from the ioc container
                using (var scope = Scope<CommerceInstance>.Begin(instance))
                {
                    var batchSize = 100;
                    var queue = instance.Database.GetRepository<ActivityQueueItem>();
                    var query = queue.Query()
                                     .Where(x => x.Status == QueueItemStatus.Pending && x.ScheduledExecuteTimeUtc <= now)
                                     .OrderBy(x => x.Id);

                    var ruleRepository = instance.Database.GetRepository<ActivityRule>();

                    foreach (var queueItem in query.Stream(batchSize))
                    {
                        // Change status
                        queueItem.MarkStarted();
                        instance.Database.Commit();

                        // Execute activity
                        using (var tx = instance.Database.BeginTransaction())
                        {
                            try
                            {
                                var @event = queueItem.LoadEvent();
                                var rule = ruleRepository.Get(queueItem.RuleId);
                                var attachedActivity = rule.AttachedActivities.ById(queueItem.AttachedActivityId);
                                var activity = activityFactory.FindByName(attachedActivity.ActivityName);

                                if (activity != null)
                                {
                                    activity.Execute(@event, new ActivityExecutionContext(rule, attachedActivity, true));
                                    // TODO: Delete queue item when success, but i think some log is needed
                                    queue.Delete(queueItem);
                                }
                                else
                                {
                                    queueItem.MarkFailed("Cannot find activity with name '" + attachedActivity.ActivityName + "'. Ensure the activity is installed.");
                                }
                            }
                            catch (Exception ex)
                            {
                                queueItem.MarkFailed(ex);
                            }

                            tx.Commit();
                        }
                    }
                }
            }

            InProgress = false;
        }
    }
}
