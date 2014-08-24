using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Scheduling
{
    public static class Schedulers
    {
        static readonly Dictionary<string, Scheduler> _schedulers = new Dictionary<string, Scheduler>();

        public static Scheduler Get(string instance)
        {
            return _schedulers[instance];
        }

        public static Scheduler Start(string instance)
        {
            if (_schedulers.ContainsKey(instance))
                throw new InvalidOperationException("Scheduler for instance '" + instance + "' already started.");

            var scheduler = new Scheduler();
            _schedulers.Add(instance, scheduler);

            return scheduler;
        }

        public static void Stop(string instance)
        {
            var scheduler = _schedulers[instance];
            _schedulers.Remove(instance);
            scheduler.UnscheduleAll();
        }
    }
}