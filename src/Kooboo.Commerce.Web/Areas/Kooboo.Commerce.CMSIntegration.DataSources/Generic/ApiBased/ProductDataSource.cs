using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class ProductDataSource : ApiBasedDataSource
    {
        public ProductDataSource()
            : base("Products", typeof(IProductQuery), typeof(Product))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Products;
        }
    }
}