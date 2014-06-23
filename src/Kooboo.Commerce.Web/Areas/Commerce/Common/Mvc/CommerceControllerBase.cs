using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public class CommerceControllerBase : Controller
    {
        [Inject]
        public CommerceInstanceContext CommerceContext { get; set; }

        protected AjaxFormResult AjaxForm()
        {
            return new AjaxFormResult(ModelState);
        }

        protected JsonNetResult JsonNet(object data)
        {
            return new JsonNetResult() { Data = data };
        }

        protected void LoadTabPlugins()
        {
            var plugins = TabPlugins.LoadTabPlugins(ControllerContext);
            ViewData["LoadedTabPlugins"] = plugins;
        }
    }
}
