using System.Linq;
using Kooboo.Commerce.Api.Categories;
using System.Collections.Generic;
using Kooboo.Commerce.Api.Local.Mapping;
using Core = Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Api.Local.Categories
{
    public class CategoryApi : ICategoryApi
    {
        private LocalApiContext _context;

        public CategoryApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<Category> Query()
        {
            return new Query<Category>(new CategoryQueryExecutor(_context));
        }

        public IList<Category> Breadcrumb(int currentCategoryId)
        {
            var tree = Core.CategoryTree.Get(_context.InstanceName);
            var currentCategory = tree.Find(currentCategoryId);

            var breadcrumb = new List<Category>();
            foreach (var category in currentCategory.PathFromRoot())
            {
                var model = ObjectMapper.Map<Core.CategoryTreeNode, Category>(category, _context, null);
                breadcrumb.Add(model);
            }

            return breadcrumb;
        }
    }
}
