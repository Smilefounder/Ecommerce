using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class ProductAPI : IProductAPI
    {
        private IProductService _productService;

        public ProductAPI(IProductService productService)
        {
            _productService = productService;
        }

        public IPagedList<Product> SearchProducts(string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50)
        {
            return _productService.GetAllProducts(userInput, categoryId, pageIndex, pageSize);
        }

        public Product GetProductById(int id)
        {
            return _productService.GetById(id);
        }
    }
}
