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
            return ApiContext.Database.GetRepository<Core.Brand>().Query();
        }

        protected override IQueryable<Core.Brand> ApplyFilter(IQueryable<Core.Brand> query, QueryFilter filter)
        {
            if (filter.Name == BrandFilters.ById.Name)
            {
                var brandId = (int)filter.Parameters["Id"];
                query = query.Where(it => it.Id == brandId);
            }
            else if (filter.Name == BrandFilters.ByName.Name)
            {
                var brandName = (string)filter.Parameters["Name"];
                query = query.Where(it => it.Name == brandName);
            }
            else if (filter.Name == BrandFilters.ByCustomField.Name)
            {
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(it => it.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            }

            return query;
        }
    }
}
