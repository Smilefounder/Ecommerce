using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public class TabSubmitContext
    {
        public object Model { get; private set; }

        public ControllerContext ControllerContext { get; private set; }

        public HttpContextBase HttpContext
        {
            get
            {
                return ControllerContext.HttpContext;
            }
        }

        public TabSubmitContext(object model, ControllerContext controllerContext)
        {
            Model = model;
            ControllerContext = controllerContext;
        }
    }
}
