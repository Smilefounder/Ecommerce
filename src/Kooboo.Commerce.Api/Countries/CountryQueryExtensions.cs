using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class CountryQueryExtensions
    {
        public static Query<Country> ById(this Query<Country> query, int id)
        {
            return query.AddFilter(CountryFilters.ById.CreateFilter(new { Id = id }));
        }

        public static Query<Country> ByName(this Query<Country> query, string name)
        {
            return query.AddFilter(CountryFilters.ByName.CreateFilter(new { Name = name }));
        }

        public static Query<Country> ByTwoLetterIsoCode(this Query<Country> query, string twoLetterIsoCode)
        {
            return query.AddFilter(CountryFilters.ByTwoLetterIsoCode.CreateFilter(new { TwoLetterIsoCode = twoLetterIsoCode }));
        }

        public static Query<Country> ByThreeLetterIsoCode(this Query<Country> query, string threeLetterIsoCode)
        {
            return query.AddFilter(CountryFilters.ByThreeLetterIsoCode.CreateFilter(new { ThreeLetterIsoCode = threeLetterIsoCode }));
        }
    }
}
