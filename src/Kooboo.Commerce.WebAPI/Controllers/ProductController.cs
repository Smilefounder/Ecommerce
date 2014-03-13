using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ProductController : CommerceAPIControllerBase
    {
        public Product Get(int id)
        {
            return Commerce().Product.GetProductById(id);
        }

        [HttpGet]
        public PageListWrapper<Product> Search(string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50)
        {
            var pagedData = Commerce().Product.SearchProducts(userInput, categoryId, pageIndex, pageSize);
            return new PageListWrapper<Product>(pagedData);
        }
    }
}
