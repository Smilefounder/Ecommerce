using System.Linq;
using Kooboo.Commerce.Api.Products;

namespace Kooboo.Commerce.Api.Local.Products
{
    public class ProductApi : IProductApi
    {
        private LocalApiContext _context;

        public ProductApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<Product> Query()
        {
            return new Query<Product>(new ProductQueryExecutor(_context));
        }
    }
}
