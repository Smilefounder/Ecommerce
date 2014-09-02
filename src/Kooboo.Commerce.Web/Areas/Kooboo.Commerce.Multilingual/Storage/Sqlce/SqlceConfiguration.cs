using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    static class SqlceConfiguration
    {
        public static void Configure()
        {
            // Ensure sqlce db folder for each instances
            var instances = EngineContext.Current.Resolve<ICommerceInstanceManager>().GetInstances().ToList();
            foreach (var instance in instances)
            {
                var folder = DataFolders.Instances.GetFolder(instance.Name).GetFolder("Multilingual");
                if (!folder.Exists)
                {
                    folder.Create();
                }
            }

            // Configure DbConfiguration
            DbConfiguration.Loaded += DbConfiguration_Loaded;

            // Register stores
            foreach (var instance in instances)
            {
                LanguageStores.Register(instance.Name, new CachedLanguageStore(new SqlceLanguageStore(instance.Name)));
                TranslationStores.Register(instance.Name, new CachedTranslactionStore(new SqlceTranslationStore(instance.Name)));
            }
        }

        static void DbConfiguration_Loaded(object sender, System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationLoadedEventArgs e)
        {
            e.AddDependencyResolver(new SingletonDependencyResolver<DbProviderServices>(
                SqlCeProviderServices.Instance, SqlCeProviderServices.ProviderInvariantName), true);
        }
    }
}