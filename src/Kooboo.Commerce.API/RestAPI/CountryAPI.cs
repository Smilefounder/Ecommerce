using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Locations.Services;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(ICountryAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class CountryAPI : RestApiBase, ICountryAPI
    {
        public IEnumerable<Country> GetAllCountries()
        {
            return Get<List<Country>>(null);
        }

        protected override string ApiControllerPath
        {
            get { return "Country"; }
        }
    }
}
