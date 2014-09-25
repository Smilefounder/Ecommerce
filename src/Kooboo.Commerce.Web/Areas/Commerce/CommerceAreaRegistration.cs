using Kooboo.CMS.Common;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System.IO;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce
{
    public class CommerceAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get
            {
                return "Commerce";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.IgnoreRoute("{allashx}", new { allashx = @".*\.ashx(\?.*)?" });
            context.MapRoute(
               this.GetType().Namespace + "_" + AreaName + "_default",
                AreaName + "/{controller}/{action}",
                new { controller = "Home", action = "Index" }
                , null
                , new[] { "Kooboo.Commerce.Web.Areas.Commerce.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Kooboo.Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));

            base.RegisterArea(context);
        }
    }
}
