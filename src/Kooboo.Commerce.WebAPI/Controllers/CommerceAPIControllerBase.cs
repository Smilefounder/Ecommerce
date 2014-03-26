using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    /// <summary>
    /// commerce api base controller
    /// </summary>
    public abstract class CommerceAPIControllerBase : ApiController
    {
        /// <summary>
        /// create the commerce api from current request context, by the commerce instance name and language
        /// </summary>
        /// <returns>commerce api</returns>
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