using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Kooboo.CMS.Common;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System.IO;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI
{
    public class CommerceWebAPIAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get
            {
                return "CommerceWebAPI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: this.GetType().Namespace + "_" + AreaName + "_default",
                url: "apiinfo/{action}/{id}",
                defaults: new { area = AreaName, controller = "ApiInfo", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers" }
            );

            var config = GlobalConfiguration.Configuration;
            config.Routes.MapHttpRoute(
                name: "DefaultGetApi",
                routeTemplate: "api/{instance}/{controller}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultPostApi",
                routeTemplate: "api/{instance}/{controller}/{id}",
                defaults: new { action = "Post", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultPutApi",
                routeTemplate: "api/{instance}/{controller}/{id}",
                defaults: new { action = "Put", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultDeleteApi",
                routeTemplate: "api/{instance}/{controller}/{id}",
                defaults: new { action = "Delete", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{instance}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            base.RegisterArea(context);
        }
    }
}
