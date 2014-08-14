using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class ProductQueryExtensions
    {
        public static Query<Product> ById(this Query<Product> query, int id)
        {
            return query.AddFilter(ProductFilters.ById.CreateFilter(new { Id = id }));
        }

        public static Query<Product> ByName(this Query<Product> query, string name)
        {
            return query.AddFilter(ProductFilters.ByName.CreateFilter(new { Name = name }));
        }

        public static Query<Product> ByCategory(this Query<Product> query, int categoryId)
        {
            return query.AddFilter(ProductFilters.ByCategory.CreateFilter(new { CategoryId = categoryId }));
        }

        public static Query<Product> ByBrand(this Query<Product> query, int brandId)
        {
            return query.AddFilter(ProductFilters.ByBrand.CreateFilter(new { BrandId = brandId }));
        }

        public static Query<Product> ByCustomField(this Query<Product> query, string fieldName, string fieldValue)
        {
            return query.AddFilter(ProductFilters.ByCustomField.CreateFilter(new { FieldName = fieldName, FieldValue = fieldValue }));
        }
    }
}
