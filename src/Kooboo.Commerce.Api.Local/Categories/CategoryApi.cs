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
            var breadcrumb = new List<Category>();
            var category = _context.Services.Categories.GetById(currentCategoryId);
            while (category != null)
            {
                breadcrumb.Add(ObjectMapper.Map<Core.Category, Category>(category, _context, null));

                if (category.Parent == null)
                {
                    break;
                }
                else
                {
                    category = category.Parent;
                }
            }

            breadcrumb.Reverse();

            return breadcrumb;
        }
    }
}
