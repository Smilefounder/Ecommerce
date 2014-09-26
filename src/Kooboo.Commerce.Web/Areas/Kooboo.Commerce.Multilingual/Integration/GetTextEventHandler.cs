using System;
using System.Linq;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Globalization.Events;
using Kooboo.Commerce.Multilingual.Storage;

namespace Kooboo.Commerce.Multilingual.Integration
{
    class GetTextEventHandler : IHandle<GetText>
    {
        public Func<string, ITranslationStore> GetTranslationStoreByInstance = instance => TranslationStores.Get(instance);

        public void Handle(GetText @event, CommerceInstance instance)
        {
            var store = GetTranslationStoreByInstance(instance.Name);

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

                    TextDictionary texts;
                    if (@event.Texts.TryGetValue(translation.EntityKey, out texts))
                    {
                        texts[prop.Property] = prop.TranslatedText;
                    }
                }
            }
        }
    }
}