using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Api.Local.Categories
{
    class CategoryQueryExecutor : QueryExecutorBase<Category, Core.CategoryTreeNode>
    {
        public CategoryQueryExecutor(LocalApiContext apiContext)
            : base(apiContext)
        {
        }

        protected override IQueryable<Core.CategoryTreeNode> CreateLocalQuery()
        {
            var tree = Core.CategoryTree.Get(ApiContext.Instance).Localize(ApiContext.Culture);
            return tree.Desendants().AsQueryable();
        }

        protected override IQueryable<Core.CategoryTreeNode> ApplyFilters(IQueryable<Core.CategoryTreeNode> query, IEnumerable<QueryFilter> filters)
        {
            query = base.ApplyFilters(query, filters);

            // We expose a category tree to client, not just a simply list all categories.
            // So if no filter is specified, we apply a ParentId = null filter implicitly, so return only root categories.
            if (!filters.Any())
            {
                query = ApplyFilter(query, CategoryFilters.ByParent.CreateFilter(new { ParentId = (int?)null }));
            }

            return query;
        }

        protected override IQueryable<Core.CategoryTreeNode> ApplyFilter(IQueryable<Core.CategoryTreeNode> query, QueryFilter filter)
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
                    query = query.Where(c => c.Parent != null && c.Parent.Id == parentId.Value);
                }
                else
                {
                    query = query.Where(c => c.Parent == null);
                }
            }
            else if (filter.Name == CategoryFilters.ByCustomField.Name)
            {
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(c => c.CustomFields.ContainsKey(fieldName) && c.CustomFields[fieldName] == fieldValue);
            }

            return query;
        }
    }
}
