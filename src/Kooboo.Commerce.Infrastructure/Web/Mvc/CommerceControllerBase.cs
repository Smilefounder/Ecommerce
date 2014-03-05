using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc.Controllers
{
    public class CommerceControllerBase : Controller
    {
        [Inject]
        public CommerceInstanceContext CommerceContext { get; set; }

        protected AjaxFormResult AjaxForm()
        {
            return new AjaxFormResult(ModelState);
        }

        protected JsonResult JsonNet(object data)
        {
            return new JsonNetResult() { Data = data };
        }
    }
}
