using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Locations
{
    public interface ICountryQuery : ICommerceQuery<Country>, ICommerceAccess<Country>
    {
        ICountryQuery ById(int id);
        ICountryQuery ByName(string name);
        ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode);
        ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode);
        ICountryQuery ByNumericISOCode(string numericISOCode);
    }
}
