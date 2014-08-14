using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Api.Local.Products
{
    class ProductQueryExecutor : QueryExecutorBase<Product, Core.Product>
    {
        public ProductQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.Product> CreateLocalQuery()
        {
            return ApiContext.Services.Products.Query().OrderByDescending(p => p.Id);
        }

        protected override IQueryable<Core.Product> ApplyFilter(IQueryable<Core.Product> query, QueryFilter filter)
        {
            if (filter.Name == ProductFilters.ById.Name)
            {
                query = query.Where(p => p.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == ProductFilters.ByName.Name)
            {
                query = query.Where(p => p.Name == (string)filter.Parameters["Name"]);
            }
            else if (filter.Name == ProductFilters.ByCategory.Name)
            {
                query = query.Where(p => p.Categories.Any(c => c.CategoryId == (int)filter.Parameters["CategoryId"]));
            }
            else if (filter.Name == ProductFilters.ByBrand.Name)
            {
                query = query.Where(p => p.BrandId == (int)filter.Parameters["BrandId"]);
            }
            else if (filter.Name == ProductFilters.ByCustomField.Name)
            {
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(p => p.CustomFields.Any(f => f.FieldName == fieldName && f.FieldValue == fieldValue));
            }

            return query;
        }
    }
}
