using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Brands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(BrandDataSource))]
    public class BrandDataSource : ApiBasedDataSource<Brand>
    {
        public override string Name
        {
            get
            {
                return "Brands";
            }
        }

        protected override Query<Brand> Query(ICommerceApi api)
        {
            return api.Brands.Query();
        }
    }
}