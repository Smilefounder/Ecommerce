using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    public interface ICountryApi
    {
        Query<Country> Query();
    }
}
