using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public class TabLoadContext
    {
        private DynamicViewDataDictionary _dynamicViewDataDictionary;

        public dynamic ViewBag
        {
            get
            {
                if (_dynamicViewDataDictionary == null)
                {
                    _dynamicViewDataDictionary = new DynamicViewDataDictionary(() => ViewData);
                }

                return _dynamicViewDataDictionary;
            }
        }

        public ViewDataDictionary ViewData { get; private set; }

        public object Model
        {
            get
            {
                return ViewData.Model;
            }
            set
            {
                ViewData.Model = value;
            }
        }

        public ControllerContext ControllerContext { get; private set; }

        public HttpContextBase HttpContext
        {
            get
            {
                return ControllerContext.HttpContext;
            }
        }

        public HttpRequestBase Request
        {
            get
            {
                return HttpContext.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return HttpContext.Response;
            }
        }

        public TabLoadContext(ControllerContext controllerContext)
        {
            Require.NotNull(controllerContext, "controllerContext");

            ControllerContext = controllerContext;
            ViewData = new ViewDataDictionary();
        }
    }
}
