using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class BrandDataSource : ApiBasedDataSource
    {
        public BrandDataSource()
            : base("Brands", typeof(IBrandQuery), typeof(Brand))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Brands;
        }
    }
}