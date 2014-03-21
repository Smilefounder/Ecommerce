using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Locations
{
    [Dependency(typeof(ICountryAPI), ComponentLifeStyle.Transient)]
    public class CountryAPI : RestApiQueryBase<Country>, ICountryAPI
    {
        public ICountryQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public ICountryQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }
        public ICountryQuery ByThreeLetterISOCode(string threeLetterISOCode)
        {
            QueryParameters.Add("threeLetterISOCode", threeLetterISOCode);
            return this;
        }

        public ICountryQuery ByTwoLetterISOCode(string twoLetterISOCode)
        {
            QueryParameters.Add("twoLetterISOCode", twoLetterISOCode);
            return this;
        }

        public ICountryQuery ByNumericISOCode(string numericISOCode)
        {
            QueryParameters.Add("numericISOCode", numericISOCode);
            return this;
        }

        public ICountryQuery Query()
        {
            return this;
        }
    }
}
