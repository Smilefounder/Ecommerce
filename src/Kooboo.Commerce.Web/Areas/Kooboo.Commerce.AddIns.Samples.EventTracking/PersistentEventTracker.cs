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
    [AwaitTransactionComplete]
    public class PersistentEventTracker// : IHandle<EntityCreated>, IHandle<EntityDeleted>, IHandle<EntityUpdated>
    {
        public void Handle(EntityCreated @event, EventDispatchingContext context)
        {
            Log("Entity created: " + @event.Entity.GetType());
        }

        public void Handle(EntityDeleted @event, EventDispatchingContext context)
        {
            Log("Entity deleted: " + @event.Entity.GetType());
        }

        public void Handle(EntityUpdated @event, EventDispatchingContext context)
        {
            Log("Entity updated: " + @event.Entity.GetType());
        }

        private void Log(string message)
        {
            var filePath = @"D:\kooboo-eventlog.txt";
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