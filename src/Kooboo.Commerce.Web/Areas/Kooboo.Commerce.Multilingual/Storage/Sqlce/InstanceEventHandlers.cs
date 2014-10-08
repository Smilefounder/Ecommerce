using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
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
        public void Handle(CommerceInstanceCreated @event, EventContext context)
        {
            var folder = DataFolders.Instances.GetFolder(@event.Settings.Name).GetFolder("Multilingual");
            folder.Create();

            var instanceManager = EngineContext.Current.Resolve<ICommerceInstanceManager>();

            // Register stores
            var instance = context.Instance;
            LanguageStores.Register(@event.InstanceName, new CachedLanguageStore(new SqlceLanguageStore(instance.Name, instanceManager)));
            TranslationStores.Register(@event.InstanceName, new CachedTranslactionStore(new SqlceTranslationStore(instance.Name, instanceManager)));
        }

        public void Handle(CommerceInstanceDeleted @event, EventContext context)
        {
            LanguageStores.Remove(@event.InstanceName);
            TranslationStores.Remove(@event.InstanceName);
        }
    }
}