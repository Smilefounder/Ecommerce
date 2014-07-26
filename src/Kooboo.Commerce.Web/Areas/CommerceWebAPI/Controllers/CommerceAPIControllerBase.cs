﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
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
        protected ICommerceApi Commerce()
        {
            // TODO: Fix this when get time
            throw new NotImplementedException();
            //var commerceService = EngineContext.Current.Resolve<ICommerceAPI>();
            //string commerceInstance = this.ControllerContext.RouteData.Values["instance"].ToString();
            //string language = "";
            //string currency = "";
            //commerceService.InitCommerceInstance(commerceInstance, language, currency, null);
            //return commerceService;
        }
	}
}