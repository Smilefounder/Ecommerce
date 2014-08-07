using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.Commerce.CMSIntegration.DataSources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Controllers
{
    [Authorize]
    public class GenericDataSourceController : Controller
    {
        private List<GenericCommerceDataSource> _dataSources;

        public GenericDataSourceController(IEnumerable<ICommerceDataSource> dataSources)
        {
            if (dataSources == null)
                throw new ArgumentNullException("dataSources");

            _dataSources = dataSources.OfType<GenericCommerceDataSource>().ToList();
        }

        public ActionResult List()
        {
            var descriptors = _dataSources.OrderBy(x => x.Name)
                                      .Select(x => new GenericDataSourceModel(x))
                                      .ToList();
            return Json(descriptors, JsonRequestBehavior.AllowGet);
        }

        public void Test()
        {
            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cms_Data\\Sites\\Vitaminstore\\DataSources\\test.config");
            var knownTypes = new List<Type>();
            knownTypes.AddRange(EngineContext.Current.ResolveAll<IDataSourceDesigner>().Select(it => it.CreateDataSource().GetType()));
            knownTypes.AddRange(EngineContext.Current.ResolveAll<ICommerceDataSource>().Select(it => it.GetType()));

            var obj = Kooboo.CMS.Sites.Persistence.Serialization.Deserialize(typeof(DataSourceSetting), knownTypes, filePath);

            var a = 0;
        }
    }
}
