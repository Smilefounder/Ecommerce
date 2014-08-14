using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Api.Local.Categories
{
    class CategoryQueryExecutor : QueryExecutorBase<Category, Core.Category>
    {
        public CategoryQueryExecutor(LocalApiContext apiContext)
            : base(apiContext)
        {
        }

        protected override IQueryable<Core.Category> CreateLocalQuery()
        {
            return ApiContext.Services.Categories.Query().OrderBy(c => c.Id);
        }

        protected override IQueryable<Core.Category> ApplyFilter(IQueryable<Core.Category> query, QueryFilter filter)
        {
            if (filter.Name == CategoryFilters.ById.Name)
            {
                var categoryId = filter.GetParameterValueOrDefault<int>("Id");
                query = query.Where(c => c.Id == categoryId);
            }
            else if (filter.Name == CategoryFilters.ByName.Name)
            {
                var name = filter.GetParameterValueOrDefault<string>("Name");
                query = query.Where(c => c.Name == name);
            }
            else if (filter.Name == CategoryFilters.ByParent.Name)
            {
                var parentId = (int?)filter.Parameters["ParentId"];
                if (parentId != null && parentId.Value > 0)
                {
                    query = query.Where(c => c.ParentId == parentId.Value);
                }
                else
                {
                    query = query.Where(c => c.ParentId == null);
                }
            }
            else if (filter.Name == CategoryFilters.ByCustomField.Name)
            {
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(c => c.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            }

            return query;
        }
    }
}
