using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public abstract class CommerceAPIControllerBase : ApiController
    {
        protected ICommerceAPI Commerce()
        {
            var commerceService = EngineContext.Current.Resolve<ICommerceAPI>();
            string commerceInstance = this.ControllerContext.RouteData.Values["instance"].ToString();
            string language = "";
            commerceService.InitCommerceInstance(commerceInstance, language, null);
            return commerceService;
        }
	}
}