using System.Linq;
using Kooboo.Commerce.Api.Countries;

namespace Kooboo.Commerce.Api.Local.Countries
{
    public class CountryApi : LocalCommerceQuery<Country, Kooboo.Commerce.Locations.Country>, ICountryApi
    {
        public CountryApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Kooboo.Commerce.Locations.Country> CreateQuery()
        {
            return Context.Services.Countries.Query();
        }

        protected override IQueryable<Kooboo.Commerce.Locations.Country> OrderByDefault(IQueryable<Kooboo.Commerce.Locations.Country> query)
        {
            return query.OrderBy(o => o.Id);
        }

        public ICountryQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public ICountryQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }

        public ICountryQuery ByThreeLetterIsoCode(string threeLetterISOCode)
        {
            Query = Query.Where(o => o.ThreeLetterIsoCode == threeLetterISOCode);
            return this;
        }

        public ICountryQuery ByTwoLetterIsoCode(string twoLetterISOCode)
        {
            Query = Query.Where(o => o.TwoLetterIsoCode == twoLetterISOCode);
            return this;
        }

        public ICountryQuery ByNumericIsoCode(string numericISOCode)
        {
            Query = Query.Where(o => o.NumericIsoCode == numericISOCode);
            return this;
        }
    }
}
