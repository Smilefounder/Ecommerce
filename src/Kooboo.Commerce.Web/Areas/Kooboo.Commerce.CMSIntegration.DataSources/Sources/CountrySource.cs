using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class CountrySource : ApiCommerceSource
    {
        public CountrySource()
            : base("Countries", typeof(ICountryQuery), typeof(Country))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Countries;
        }
    }
}