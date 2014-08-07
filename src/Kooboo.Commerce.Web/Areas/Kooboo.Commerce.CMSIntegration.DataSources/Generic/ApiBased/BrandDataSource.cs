using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(BrandDataSource))]
    public class BrandDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "Brands"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IBrandQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(Brand); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Brands;
        }
    }
}