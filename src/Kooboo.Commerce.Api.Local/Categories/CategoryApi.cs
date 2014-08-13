using System.Linq;
using Kooboo.Commerce.Api.Categories;

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
    }
}
