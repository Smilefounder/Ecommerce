using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CommerceAPIControllerBase : ApiController
    {
        protected ICommerceAPI Commerce()
        {
            var api = "LocalCommerceAPI";
            var commerceService = EngineContext.Current.Resolve<ICommerceAPI>(api);
            string commerceInstance = this.ControllerContext.RouteData.Values["instance"].ToString();
            string language = "";
            commerceService.InitCommerceInstance(commerceInstance, language);
            return commerceService;
        }
	}
}