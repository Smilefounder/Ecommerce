using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class Scheduler
    {
        private readonly object _lock = new object();
        private Dictionary<string, Schedule> _schedules = new Dictionary<string, Schedule>();

        public IEnumerable<Schedule> Schedules
        {
            get
            {
                return _schedules.Values;
            }
        }

        public string Instance { get; private set; }

        public Scheduler(string instance)
        {
            Instance = instance;
        }

        public Schedule GetSchedule(string jobName)
        {
            Schedule schedule;
            if (_schedules.TryGetValue(jobName, out schedule))
            {
                return schedule;
            }

            return null;
        }

        public void Schedule(string jobName, IJob job, TimeSpan interval, TimeOfDay startTime, IDictionary<string, string> jobData)
        {
            lock (_lock)
            {
                var schedule = new Schedule(Instance, jobName, job, interval, startTime, jobData);
                _schedules.Add(jobName, schedule);
                schedule.Start();
            }
        }

        public void Reschedule(string jobName, TimeSpan interval, TimeOfDay startTime)
        {
            var oldSchedule = GetSchedule(jobName);
            var newSchedule = oldSchedule.Clone(interval, startTime);

            lock (_lock)
            {
                Unschedule(oldSchedule.JobName, true);
                _schedules.Add(newSchedule.JobName, newSchedule);
                newSchedule.Start();
            }
        }

        public void UnscheduleAll(bool waitUnitlStopped)
        {
            lock (_lock)
            {
                foreach (var jobId in _schedules.Keys.ToList())
                {
                    Unschedule(jobId, waitUnitlStopped);
                }
            }
        }

        public void Unschedule(string jobName, bool waitUnitlStopped)
        {
            lock (_lock)
            {
                if (_schedules.ContainsKey(jobName))
                {
                    var schedule = _schedules[jobName];
                    _schedules.Remove(jobName);
                    schedule.Stop(waitUnitlStopped);
                }
            }
        }
    }
}