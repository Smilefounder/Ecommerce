using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using Kooboo.Commerce.Web.Framework.UI.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TabPluginController : CommerceController
    {
        [HttpPost, HandleAjaxError, Transactional]
        public void Submit(string pluginType, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object model)
        {
            var type = Type.GetType(pluginType, true);
            var plugin = (ITabPlugin)EngineContext.Current.Resolve(type);
            plugin.OnSubmit(new TabSubmitContext(model, ControllerContext));
        }
    }
}
