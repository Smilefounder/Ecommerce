using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [Dependency(typeof(IDataSourceDesigner), Key = "Commerce datasource")]
    public class CommerceDataSourceDesigner : IDataSourceDesigner
    {
        public IDataSource CreateDataSource()
        {
            return new CommerceDataSourceAdapter();
        }

        public string DesignerVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/DataSourceDesigner.cshtml";
            }
        }

        public bool IsEditorFor(IDataSource dataSource)
        {
            return dataSource is CommerceDataSourceAdapter;
        }

        public string Name
        {
            get
            {
                return "Commerce datasource";
            }
        }
    }
}