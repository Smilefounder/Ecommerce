using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(CountryDataSource))]
    public class CountryDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "Countries"; }
        }

        protected override Type QueryType
        {
            get { return typeof(ICountryQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(Country); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Countries;
        }
    }
}