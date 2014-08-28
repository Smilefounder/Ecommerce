using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class Schedule : IDisposable
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IJob _job;
        private Thread _thread;
        private int _secondsToWait;
        private AutoResetEvent _event;
        private bool _stopRequested;

        public string Instance { get; private set; }

        public string JobName { get; private set; }

        public TimeOfDay StartTime { get; private set; }

        public TimeSpan Interval { get; private set; }

        public IDictionary<string, string> JobData { get; private set; }

        public Schedule(string instance, string jobName, IJob job, TimeSpan interval, TimeOfDay startTime, IDictionary<string, string> jobData)
        {
            Instance = instance;
            JobName = jobName;
            _job = job;
            JobData = jobData;
            Interval = interval;
            StartTime = startTime;
            _thread = new Thread(DoWork);
            _event = new AutoResetEvent(false);
        }

        public Schedule Clone(TimeSpan newInterval, TimeOfDay newStartTime)
        {
            return new Schedule(Instance, JobName, _job, newInterval, newStartTime, JobData);
        }

        public void Start()
        {
            var now = DateTime.Now;
            var startTime = DateTime.Today.AddHours(StartTime.Hour).AddMinutes(StartTime.Mintue);

            if (startTime >= now)
            {
                _secondsToWait = (int)(startTime - now).TotalSeconds;
            }
            else
            {
                _secondsToWait = (int)(now - startTime).TotalSeconds % (int)Interval.TotalSeconds;
            }

            _thread.Start();
        }

        public void Stop(bool waitUnitlStopped)
        {
            _stopRequested = true;
            _event.Set();

            if (waitUnitlStopped)
            {
                _thread.Join();
            }
        }

        private void DoWork()
        {
            while (true)
            {
                _event.WaitOne(_secondsToWait * 1000);

                if (_stopRequested)
                {
                    break;
                }

                _secondsToWait = (int)Interval.TotalSeconds;

                _log.Info("Job '" + JobName + "' started.");

                try
                {
                    _job.Execute(new JobContext(Instance, JobData));
                    _log.Info("Job '" + JobName + "' completed.");
                }
                catch (Exception ex)
                {
                    _log.ErrorException("Faile to execute job '" + JobName + "': " + ex.Message, ex);
                }
            }

            // Dispose resource
            _event.Dispose();

            if (_job is IDisposable)
            {
                ((IDisposable)_job).Dispose();
            }
        }

        public void Dispose()
        {
            Stop(false);
        }
    }
}