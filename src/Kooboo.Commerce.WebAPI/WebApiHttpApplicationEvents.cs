using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;
using System.Web.Routing;

namespace Kooboo.Commerce.WebAPI
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "WebApiHttpApplicationEvents")]
    public class WebApiHttpApplicationEvents : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.DependencyResolver = new WebApiDependencyResolver(EngineContext.Current, GlobalConfiguration.Configuration.DependencyResolver);
            System.Web.Mvc.DependencyResolver.SetResolver(new Kooboo.CMS.Common.Runtime.Mvc.MvcDependencyResolver(EngineContext.Current, System.Web.Mvc.DependencyResolver.Current));

#if DEBUG
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
#endif
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}