using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    class DbDirectorySynchronizer : IHandle<CommerceInstanceCreated>
    {
        public void Handle(CommerceInstanceCreated @event)
        {
            var folder = DataFolders.Instances.GetFolder(@event.Settings.Name).GetFolder("Multilingual");
            folder.Create();
        }
    }
}