using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Scheduling
{
    public class Scheduler
    {
        private readonly object _lock = new object();
        private Dictionary<string, Schedule> _schedules = new Dictionary<string, Schedule>();

        public void Schedule(IJob job, TimeSpan interval, DateTime startTimeUtc)
        {
            lock (_lock)
            {
                var schedule = new Schedule(job, interval, startTimeUtc);
                _schedules.Add(job.Id, schedule);
                schedule.Start();
            }
        }

        public void UnscheduleAll()
        {
            lock (_lock)
            {
                foreach (var jobId in _schedules.Keys)
                {
                    Unschedule(jobId);
                }
            }
        }

        public void Unschedule(string jobId)
        {
            lock (_lock)
            {
                if (_schedules.ContainsKey(jobId))
                {
                    var schedule = _schedules[jobId];
                    _schedules.Remove(jobId);

                    try
                    {
                        schedule.Stop(false);
                    }
                    finally
                    {
                        schedule.Dispose();
                    }
                }
            }
        }
    }
}