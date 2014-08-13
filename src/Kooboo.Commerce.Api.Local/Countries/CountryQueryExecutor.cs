using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Countries;

namespace Kooboo.Commerce.Api.Local.Countries
{
    class CountryQueryExecutor : QueryExecutorBase<Country, Core.Country>
    {
        public CountryQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.Country> CreateLocalQuery()
        {
            return ApiContext.Services.Countries.Query().OrderBy(c => c.Id);
        }

        protected override IQueryable<Core.Country> ApplyFilter(IQueryable<Core.Country> query, QueryFilter filter)
        {
            if (filter.Name == CountryFilters.ById.Name)
            {
                query = query.Where(c => c.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == CountryFilters.ByName.Name)
            {
                query = query.Where(c => c.Name == (string)filter.Parameters["Name"]);
            }
            else if (filter.Name == CountryFilters.ByTwoLetterIsoCode.Name)
            {
                query = query.Where(c => c.TwoLetterIsoCode == (string)filter.Parameters["TwoLetterIsoCode"]);
            }
            else if (filter.Name == CountryFilters.ByThreeLetterIsoCode.Name)
            {
                query = query.Where(c => c.ThreeLetterIsoCode == (string)filter.Parameters["ThreeLetterIsoCode"]);
            }
            else if (filter.Name == CountryFilters.ByNumericIsoCode.Name)
            {
                query = query.Where(c => c.NumericIsoCode == (string)filter.Parameters["NumericIsoCode"]);
            }

            return query;
        }
    }
}
