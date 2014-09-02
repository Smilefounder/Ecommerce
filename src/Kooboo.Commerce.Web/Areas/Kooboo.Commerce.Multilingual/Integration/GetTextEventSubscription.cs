using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Globalization.Events;
using Kooboo.Commerce.Multilingual.Storage;
using System;
using System.Linq;

namespace Kooboo.Commerce.Multilingual.Integration
{
    class GetTextEventSubscription : IHandle<GetText>
    {
        public void Handle(GetText @event)
        {
            var store = TranslationStores.Get(CommerceInstance.Current.Name);

            var entityKeys = @event.Texts.Select(m => m.Key).ToArray();
            var translations = store.Find(@event.Culture, entityKeys);

            for (var i = 0; i < translations.Length; i++)
            {
                var translation = translations[i];
                if (translation == null)
                {
                    continue;
                }

                foreach (var prop in translation.PropertyTranslations)
                {
                    if (String.IsNullOrEmpty(prop.TranslatedText))
                    {
                        continue;
                    }

                    @event.SetText(translation.EntityKey, prop.Property, prop.TranslatedText);
                }
            }
        }
    }
}