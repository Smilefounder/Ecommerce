using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Locations
{
    public interface ICountryAPI : ICountryQuery
    {
        ICountryQuery Query();
    }
}
