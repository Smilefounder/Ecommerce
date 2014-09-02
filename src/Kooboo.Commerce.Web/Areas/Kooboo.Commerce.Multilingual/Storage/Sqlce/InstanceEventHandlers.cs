using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    class InstanceEventHandlers : IHandle<CommerceInstanceCreated>, IHandle<CommerceInstanceDeleted>
    {
        public void Handle(CommerceInstanceCreated @event)
        {
            var folder = DataFolders.Instances.GetFolder(@event.Settings.Name).GetFolder("Multilingual");
            folder.Create();

            // Register stores
            LanguageStores.Register(@event.InstanceName, new CachedLanguageStore(new SqlceLanguageStore(@event.InstanceName)));
            TranslationStores.Register(@event.InstanceName, new CachedTranslactionStore(new SqlceTranslationStore(@event.InstanceName)));
        }

        public void Handle(CommerceInstanceDeleted @event)
        {
            LanguageStores.Remove(@event.InstanceName);
            TranslationStores.Remove(@event.InstanceName);
        }
    }
}