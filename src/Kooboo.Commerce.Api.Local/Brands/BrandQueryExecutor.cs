using Kooboo.Commerce.Api.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.Api.Local.Brands
{
    class BrandQueryExecutor : QueryExecutorBase<Brand, Core.Brand>
    {
        public BrandQueryExecutor(LocalApiContext apiContext)
            : base(apiContext)
        {
        }

        protected override IQueryable<Core.Brand> CreateLocalQuery()
        {
            return ApiContext.Services.Brands.Query();
        }

        protected override IQueryable<Core.Brand> ApplyFilter(IQueryable<Core.Brand> query, QueryFilter filter)
        {
            if (filter.Name == BrandFilters.ById.Name)
            {
                query = query.Where(it => it.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == BrandFilters.ByName.Name)
            {
                query = query.Where(it => it.Name == (string)filter.Parameters["Name"]);
            }
            else if (filter.Name == BrandFilters.ByCustomField.Name)
            {
                query = query.Where(it => it.CustomFields.Any(f => f.Name == (string)filter.Parameters["FieldName"] && f.Value == (string)filter.Parameters["FieldValue"]));
            }

            return query;
        }
    }
}
