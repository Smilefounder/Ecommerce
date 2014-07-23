using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Data
{
    public class SiteAwareHttpCurrentInstanceProvider : HttpCurrentInstanceProvider
    {
        public SiteAwareHttpCurrentInstanceProvider(ICommerceInstanceManager instanceManager)
            : base(instanceManager)
        {
        }

        protected override string GetInstanceName(HttpContextBase httpContext)
        {
            var instance = base.GetInstanceName(httpContext);
            if (String.IsNullOrEmpty(instance))
            {
                var site = Site.Current;
                if (site != null)
                {
                    site.CustomFields.TryGetValue("CommerceInstance", out instance);
                }
            }

            return instance;
        }
    }
}