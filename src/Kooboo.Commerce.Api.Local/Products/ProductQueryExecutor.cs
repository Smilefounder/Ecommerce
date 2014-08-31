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
            return ApiContext.Services.Products.Query().Where(it => it.IsPublished).OrderByDescending(p => p.Id);
        }

        protected override IQueryable<Core.Product> ApplyFilter(IQueryable<Core.Product> query, QueryFilter filter)
        {
            if (filter.Name == ProductFilters.ById.Name)
            {
                var id = (int)filter.Parameters["Id"];
                query = query.Where(p => p.Id == id);
            }
            else if (filter.Name == ProductFilters.ByIds.Name)
            {
                var ids = (int[])filter.Parameters["Ids"];
                if (ids != null)
                {
                    query = query.Where(p => ids.Contains(p.Id));
                }
            }
            else if (filter.Name == ProductFilters.ByName.Name)
            {
                var name = (string)filter.Parameters["Name"];
                query = query.Where(p => p.Name == name);
            }
            else if (filter.Name == ProductFilters.ByCategory.Name)
            {
                var categoryId = (int)filter.Parameters["CategoryId"];
                query = query.Where(p => p.Categories.Any(c => c.Id == categoryId));
            }
            else if (filter.Name == ProductFilters.ByBrand.Name)
            {
                var brandId = (int)filter.Parameters["BrandId"];
                query = query.Where(p => p.Brand.Id == brandId);
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
