using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.UI.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public class CommerceController : Controller
    {
        public CommerceInstance CurrentInstance
        {
            get
            {
                return CommerceInstance.Current;
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Site.Current = null;
            Repository.Current = null;

            var siteName = requestContext.GetRequestValue("siteName");
            if (siteName != null)
            {
                var name = siteName.ToString();
                if (!string.IsNullOrEmpty(name))
                {
                    Site.Current = (SiteHelper.Parse(siteName)).AsActual();
                    if (Site.Current != null)
                    {
                        if (Site.Current.GetRepository() != null)
                        {
                            Repository.Current = Site.Current.GetRepository().AsActual();
                        }
                    }
                }
            }
        }

        protected AjaxFormResult AjaxForm()
        {
            return new AjaxFormResult(ModelState);
        }

        protected JsonNetResult JsonNet(object data)
        {
            return new JsonNetResult() { Data = data };
        }
    }
}
