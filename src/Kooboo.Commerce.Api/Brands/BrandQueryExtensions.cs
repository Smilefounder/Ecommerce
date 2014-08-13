using Kooboo.Commerce.Api.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class BrandQueryExtensions
    {
        public static Query<Brand> ById(this Query<Brand> query, int id)
        {
            query.Filters.Add(BrandFilters.ById.CreateFilter(new { Id = id }));
            return query;
        }

        public static Query<Brand> ByName(this Query<Brand> query, string name)
        {
            query.Filters.Add(BrandFilters.ByName.CreateFilter(new { Name = name }));
            return query;
        }

        public static Query<Brand> ByCustomField(this Query<Brand> query, string fieldName, string fieldValue)
        {
            query.Filters.Add(BrandFilters.ByCustomField.CreateFilter(new
            {
                FieldName = fieldName,
                FieldValue = fieldValue
            }));
            return query;
        }
    }
}
