using System.Linq;
using Kooboo.Commerce.Api.Countries;

namespace Kooboo.Commerce.Api.Local.Countries
{
    public class CountryApi : ICountryApi
    {
        private LocalApiContext _context;

        public CountryApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<Country> Query()
        {
            return new Query<Country>(new CountryQueryExecutor(_context));
        }
    }
}
