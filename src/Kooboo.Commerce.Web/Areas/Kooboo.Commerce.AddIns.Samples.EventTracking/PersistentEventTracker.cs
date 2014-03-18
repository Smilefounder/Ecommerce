using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.AddIns.Samples.EventTracking
{
    [AwaitDbCommit]
    public class PersistentEventTracker : IHandles<EntityAdded>, IHandles<EntityUpdated>, IHandles<EntityDeleted>
    {
        public void Handle(EntityAdded @event, EventDispatchingContext context)
        {
            Log(@event.CommerceName + ": Add " + @event.Entity.GetType().Name);
        }

        public void Handle(EntityUpdated @event, EventDispatchingContext context)
        {
            Log(@event.CommerceName + ": Update " + @event.Entity.GetType().Name);
        }

        public void Handle(EntityDeleted @event, EventDispatchingContext context)
        {
            Log(@event.CommerceName + ": Delete " + @event.Entity.GetType().Name);
        }

        private void Log(string message)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Persistent-EventLog.txt");
            var logMessage = "[" + DateTime.Now + "] " + message + Environment.NewLine;

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, logMessage, Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(filePath, logMessage);
            }
        }
    }
}