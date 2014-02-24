using System.IO;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Media
{
    public class MediaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Media";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.IgnoreRoute("{allashx}", new { allashx = @".*\.ashx(\?.*)?" });
            context.MapRoute(
                "Media_default",
                "Media/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Kooboo.Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));
        }
    }
}
