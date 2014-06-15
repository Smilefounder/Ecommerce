using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    [Dependency(typeof(ICommerceSource), Key = "ShoppingCarts")]
    public class ShoppingCartSource : ApiCommerceSource
    {
        public ShoppingCartSource()
            : base("ShoppingCarts", typeof(IShoppingCartQuery))
        {
            InternalIncludablePaths.Remove("AppliedPromotions");
        }
    }
}