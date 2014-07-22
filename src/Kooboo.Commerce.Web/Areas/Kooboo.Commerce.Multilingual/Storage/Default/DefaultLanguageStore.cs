using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Multilingual.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Default
{
    [Dependency(typeof(ILanguageStore))]
    public class DefaultLanguageStore : ILanguageStore
    {
        public Func<CommerceInstance> CurrentInstance = () => CommerceInstance.Current;

        public IEnumerable<Language> All()
        {
            return MultilingualDataFolder.GetLanguagesFolder(CurrentInstance().Name)
                                         .GetFolders()
                                         .Select(f => f.GetFile("settings.config").Read<Language>())
                                         .Where(f => f != null)
                                         .ToList();
        }

        public Language Find(string name)
        {
            return MultilingualDataFolder.GetLanguageFolder(CurrentInstance().Name, name)
                                         .GetFile("settings.config")
                                         .Read<Language>();
        }

        public void Add(Language language)
        {
            Require.NotNull(language, "language");

            var folder = MultilingualDataFolder.GetLanguageFolder(CurrentInstance().Name, language.Name);
            var settingsFile = folder.GetFile("settings.config");
            if (settingsFile.Exists)
                throw new InvalidOperationException("Language " + language.Name + " already exists.");

            settingsFile.Write(language);
        }

        public void Update(Language language)
        {
            Require.NotNull(language, "language");

            var folder = MultilingualDataFolder.GetLanguageFolder(CurrentInstance().Name, language.Name);
            var settingsFile = folder.GetFile("settings.config");
            if (!settingsFile.Exists)
                throw new InvalidOperationException("Language " + language.Name + " was not found.");

            settingsFile.Write(language);
        }

        public void Delete(string code)
        {
            var folder = MultilingualDataFolder.GetLanguageFolder(CurrentInstance().Name, code);
            if (folder.Exists)
            {
                folder.Delete();
            }
        }
    }
}