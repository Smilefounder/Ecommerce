using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.API.LocalProvider
{
    [Dependency(typeof(ICommerceAPI), ComponentLifeStyle.InRequestScope)]
    public class LocalCommerceAPI : CommerceAPIBase
    {
        public override void InitCommerceInstance(string instance, string language)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[HttpCommerceInstanceNameResolverBase.DefaultParamName] = instance;
                HttpContext.Current.Items["language"] = language;
            }
        }
    }
}
