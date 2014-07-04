using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Activities.Scheduling;
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
            var activityProvider = engine.Resolve<IActivityProvider>();
            var now = DateTime.UtcNow;

            foreach (var instance in instanceManager.GetInstances())
            {
                // Begin a scope so the activities are able to resolve dependencies from the ioc container
                using (var scope = Scope.Begin(instance))
                {
                    var batchSize = 100;
                    var queue = instance.Database.GetRepository<ScheduledActivity>();
                    var query = queue.Query()
                                     .Where(x => x.Status == ActivityExecutionStatus.Pending && x.ScheduledExecutionTimeUtc <= now)
                                     .OrderBy(x => x.Id);

                    foreach (var queueItem in query.Batched(batchSize))
                    {
                        // Change status
                        queueItem.MarkStarted();
                        instance.Database.SaveChanges();

                        // Execute activity
                        using (var tx = instance.Database.BeginTransaction())
                        {
                            try
                            {
                                var @event = queueItem.LoadEvent();
                                var activity = activityProvider.FindByName(queueItem.ActivityName);
                                if (activity != null)
                                {
                                    object parameters = null;
                                    if (activity.ConfigModelType != null)
                                    {
                                        parameters = ConfiguredActivity.LoadConfigModel(queueItem.ActivityConfig, activity.ConfigModelType);
                                    }

                                    activity.Execute(@event, new ActivityContext(parameters, true));
                                    // TODO: Delete queue item when success, but i think some log is needed
                                    queue.Delete(queueItem);
                                }
                                else
                                {
                                    queueItem.MarkFailed("Cannot find activity with name '" + queueItem.ActivityName + "'. Ensure the activity is installed.");
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
