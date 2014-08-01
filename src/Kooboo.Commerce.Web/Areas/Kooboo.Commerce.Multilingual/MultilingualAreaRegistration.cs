using Kooboo.CMS.Common;
using Kooboo.Commerce.Multilingual.Models.Mapping;
using Kooboo.Commerce.Multilingual.Storage.Sqlce;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Core.Common;

namespace Kooboo.Commerce.Multilingual
{
    public class MultilingualAreaRegistration : AddinAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(AddinAreaRegistrationContext context)
        {
            context.MapRoute(
                 name: AreaName + "_default",
                 url: AreaName + "/{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "Kooboo.Commerce.Multilingual.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            MapperConfiguration.Configure();
            SqlceConfiguration.Configure();
        }
    }
}