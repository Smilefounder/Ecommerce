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
            return ApiContext.Database.GetRepository<Core.Country>().Query().OrderBy(c => c.Id);
        }

        protected override IQueryable<Core.Country> ApplyFilter(IQueryable<Core.Country> query, QueryFilter filter)
        {
            if (filter.Name == CountryFilters.ById.Name)
            {
                var id = filter.GetParameterValueOrDefault<int>("Id");
                query = query.Where(c => c.Id == id);
            }
            else if (filter.Name == CountryFilters.ByName.Name)
            {
                var name = filter.GetParameterValueOrDefault<string>("Name");
                query = query.Where(c => c.Name == name);
            }
            else if (filter.Name == CountryFilters.ByTwoLetterIsoCode.Name)
            {
                var code = filter.GetParameterValueOrDefault<string>("TwoLetterIsoCode");
                query = query.Where(c => c.TwoLetterIsoCode == code);
            }
            else if (filter.Name == CountryFilters.ByThreeLetterIsoCode.Name)
            {
                var code = filter.GetParameterValueOrDefault<string>("ThreeLetterIsoCode");
                query = query.Where(c => c.ThreeLetterIsoCode == code);
            }
            else if (filter.Name == CountryFilters.ByNumericIsoCode.Name)
            {
                var code = filter.GetParameterValueOrDefault<string>("NumericIsoCode");
                query = query.Where(c => c.NumericIsoCode == code);
            }

            return query;
        }
    }
}
