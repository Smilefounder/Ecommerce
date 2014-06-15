using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    [Dependency(typeof(ICommerceSource), Key = "Products")]
    public class ProductSource : ApiCommerceSource
    {
        public ProductSource()
            : base("Products", typeof(IProductQuery))
        {
        }
    }
}