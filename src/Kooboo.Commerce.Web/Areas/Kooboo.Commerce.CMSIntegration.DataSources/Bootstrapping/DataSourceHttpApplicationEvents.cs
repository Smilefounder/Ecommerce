using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.CMSIntegration.DataSources.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Bootstrapping
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "DataSourceHttpApplicationEvents")]
    public class DataSourceHttpApplicationEvents : Kooboo.CMS.Common.Runtime.Mvc.MvcModule
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(ICommerceDataSource), new CommerceDataSourceModelBinder());
        }
    }
}