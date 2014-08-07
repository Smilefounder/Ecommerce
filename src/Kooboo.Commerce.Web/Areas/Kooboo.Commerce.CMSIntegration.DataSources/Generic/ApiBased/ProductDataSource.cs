using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ProductDataSource))]
    public class ProductDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "Products"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IProductQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(Product); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Products;
        }
    }
}