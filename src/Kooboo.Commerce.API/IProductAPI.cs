using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.API
{
    public interface IProductAPI
    {
        IPagedList<Product> SearchProducts(string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50);

        Product GetProductById(int id);
    }
}
