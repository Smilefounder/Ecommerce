using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Locations;

namespace Kooboo.Commerce.API
{
    public interface ICountryAPI
    {
        IEnumerable<Country> GetAllCountries();
    }
}
