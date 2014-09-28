using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Persistence
{
    /// <summary>
    /// Customized DataSourceSettingProvider which is aware of ICommerceDataSource types when serializing/deserializing data source settings.
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IDataSourceSettingProvider), Order = 200)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<DataSourceSetting>), Order = 200)]
    public class CommerceDataSourceSettingProvider : InheritableProviderBase<DataSourceSetting>, IDataSourceSettingProvider
    {
        #region .ctor
        const string DIRNAME = "DataSources";
        static System.Threading.ReaderWriterLockSlim @lock = new System.Threading.ReaderWriterLockSlim();
        IDataSourceDesigner[] _designers;
        CommerceDataSource[] _commerceDataSources;

        public CommerceDataSourceSettingProvider(IDataSourceDesigner[] designers, CommerceDataSource[] commerceDataSources)
        {
            _designers = designers;
            _commerceDataSources = commerceDataSources;
        }
        #endregion
        public void Localize(DataSourceSetting o, Site targetSite)
        {
        }

        protected override IFileStorage<DataSourceSetting> GetFileStorage(Site site)
        {
            var basePath = Path.Combine(site.PhysicalPath, DIRNAME);
            var knownTypes = _designers.Where(it => !(it is CommerceDataSourceDesigner)).Select(it => it.CreateDataSource().GetType()).ToList();
            knownTypes.AddRange(_commerceDataSources.Select(it => it.GetType()));

            return new XmlObjectFileStorage<DataSourceSetting>(basePath, @lock, knownTypes);
        }
    }
}