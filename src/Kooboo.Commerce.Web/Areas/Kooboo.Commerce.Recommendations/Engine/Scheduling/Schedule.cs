using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Scheduling
{
    public class Schedule : IDisposable
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IJob _job;
        private Thread _thread;
        private int _secondsToWait;
        private AutoResetEvent _event;
        private bool _stopRequested;

        public DateTime StartTimeUtc { get; private set; }

        public TimeSpan Interval { get; private set; }

        public Schedule(IJob job, TimeSpan interval, DateTime startTimeUtc)
        {
            _job = job;
            Interval = interval;
            StartTimeUtc = startTimeUtc;
            _thread = new Thread(DoWork);
            _event = new AutoResetEvent(false);
        }

        public void Start()
        {
            var now = DateTime.UtcNow;

            if (StartTimeUtc >= now)
            {
                _secondsToWait = (int)(StartTimeUtc - now).TotalSeconds;
            }
            else
            {
                _secondsToWait = (int)(now - StartTimeUtc).TotalSeconds % (int)Interval.TotalSeconds;
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

                _log.Info("Job '" + _job.Id + "' started.");

                try
                {
                    _job.Execute();
                    _log.Info("Job '" + _job.Id + "' completed.");
                }
                catch (Exception ex)
                {
                    _log.ErrorException("Faile to execute job " + _job.Id + ": " + ex.Message, ex);
                }
            }
        }

        public void Dispose()
        {
            _event.Dispose();

            if (_job is IDisposable)
            {
                ((IDisposable)_job).Dispose();
            }
        }
    }
}