using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class CategoryQueryExtensions
    {
        public static Query<Category> ById(this Query<Category> query, int id)
        {
            query.Filters.Add(CategoryFilters.ById.CreateFilter(new { Id = id }));
            return query;
        }

        public static Query<Category> ByName(this Query<Category> query, string name)
        {
            query.Filters.Add(CategoryFilters.ByName.CreateFilter(new { Name = name }));
            return query;
        }

        public static Query<Category> ByParent(this Query<Category> query, int? parentId)
        {
            query.Filters.Add(CategoryFilters.ByParent.CreateFilter(new { ParentId = parentId }));
            return query;
        }

        public static Query<Category> ByCustomField(this Query<Category> query, string fieldName, string fieldValue)
        {
            query.Filters.Add(CategoryFilters.ByCustomField.CreateFilter(new { FieldName = fieldName, FieldValue = fieldValue }));
            return query;
        }
    }
}
