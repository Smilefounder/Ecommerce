using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Locations;
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
    }
}