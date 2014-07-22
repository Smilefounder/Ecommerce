using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Multilingual.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Default
{
    [Dependency(typeof(ITranslationStore))]
    public class DefaultTranslationStore : ITranslationStore
    {
        public Func<CommerceInstance> CurrentInstance = () => CommerceInstance.Current;

        public EntityTransaltion[] Find(System.Globalization.CultureInfo culture, params Globalization.EntityKey[] keys)
        {
            var result = new EntityTransaltion[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var propTexts = GetTranslationDataFile(key, culture).Read<IDictionary<string, string>>();
                result[i] = new EntityTransaltion(culture.Name, key, propTexts);
            }

            return result;
        }

        public void AddOrUpdate(CultureInfo culture, EntityKey key, IDictionary<string, string> propertyTranslations)
        {
            var file = GetTranslationDataFile(key, culture);
            var translations = file.Read<TextTranslationDictionary>() ?? new TextTranslationDictionary();
            foreach (var each in propertyTranslations)
            {
                translations[each.Key] = each.Value;
            }

            file.Write(translations);
        }

        private DataFile GetTranslationDataFile(EntityKey key, CultureInfo culture)
        {
            var folder = MultilingualDataFolder.GetLanguageFolder(CurrentInstance().Name, culture.Name)
                                               .GetFolder(key.EntityType.Name);

            var keyType = key.Value.GetType();
            if (keyType == typeof(Int32) || keyType == typeof(Int64))
            {
                // Optimize for int32 and int64 (Int64 max value has 19 digits)
                var id = Convert.ToInt64(key.Value);

                var pow = 18;
                var leftValue = id;

                while (pow > 0)
                {
                    var temp = (Int64)Math.Pow(10, pow);
                    var folderName = (leftValue / temp).ToString().PadLeft(3, '0');
                    leftValue = leftValue % temp;
                    folder = folder.GetFolder(folderName);
                    pow -= 3;
                }

                return folder.GetFile(key.Value + ".config");
            }
            else
            {
                return folder.GetFile(key.Value + ".config");
            }
        }
    }
}