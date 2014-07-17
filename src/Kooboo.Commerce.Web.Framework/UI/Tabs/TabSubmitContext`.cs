using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public class TabSubmitContext<TModel>
    {
        public TabSubmitContext _innerContext;

        public ControllerContext ControllerContext
        {
            get
            {
                return _innerContext.ControllerContext;
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                return _innerContext.HttpContext;
            }
        }

        public TModel Model
        {
            get
            {
                return (TModel)_innerContext.Model;
            }
        }

        public TabSubmitContext(TabSubmitContext context)
        {
            Require.NotNull(context, "context");
            _innerContext = context;
        }
    }
}
