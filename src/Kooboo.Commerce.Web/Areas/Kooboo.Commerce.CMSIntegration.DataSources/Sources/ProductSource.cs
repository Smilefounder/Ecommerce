using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class ProductSource : ApiCommerceSource
    {
        public ProductSource()
            : base("Products", typeof(IProductQuery), typeof(Product))
        {
        }

        protected override object GetQuery(API.ICommerceAPI api)
        {
            return api.Products;
        }
    }
}