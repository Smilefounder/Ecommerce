using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Locations.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(ICountryAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class CountryAPI : ICountryAPI
    {
        private ICountryService _countryService;

        public CountryAPI(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _countryService.GetAllCountries();
        }
    }
}
