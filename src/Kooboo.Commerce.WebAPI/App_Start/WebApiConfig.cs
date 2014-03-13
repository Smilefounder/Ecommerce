using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Kooboo.Commerce.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultGetApi",
                routeTemplate: "{instance}/{controller}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultPostApi",
                routeTemplate: "{instance}/{controller}/{id}",
                defaults: new { action = "Post", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultPutApi",
                routeTemplate: "{instance}/{controller}/{id}",
                defaults: new { action = "Put", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultDeleteApi",
                routeTemplate: "{instance}/{controller}/{id}",
                defaults: new { action = "Delete", id = RouteParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete), id = @"^\d*$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "{instance}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}
