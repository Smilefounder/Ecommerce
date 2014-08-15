using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(CountryDataSource))]
    public class CountryDataSource : ApiQueryBasedDataSource<Country>
    {
        public override string Name
        {
            get { return "Countries"; }
        }

        protected override Query<Country> Query(ICommerceApi api)
        {
            return api.Countries.Query();
        }
    }
}