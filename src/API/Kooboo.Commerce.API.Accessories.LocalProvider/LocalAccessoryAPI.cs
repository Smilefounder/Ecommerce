using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Accessories;
using Kooboo.Commerce.API.LocalProvider;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Products.Services;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Commerce.API.Accessories.LocalProvider
{
    [Dependency(typeof(IAccessoryAPI))]
    public class LocalAccessoryAPI : IAccessoryAPI
    {
        private IProductService _productService;
        private IProductAccessoryService _accessoryService;
        private IMapper<Product, Kooboo.Commerce.Products.Product> _mapper;

        public LocalAccessoryAPI(
            IProductService productService,
            IProductAccessoryService accessoryService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
        {
            _productService = productService;
            _accessoryService = accessoryService;
            _mapper = mapper;
        }

        public Product[] ForProduct(int productId)
        {
            var accessories = _accessoryService.GetAccessories(productId);
            var accessoryIds = accessories.Select(x => x.ProductId).ToArray();
            var products = _productService.Query()
                                          .Where(x => accessoryIds.Contains(x.Id))
                                          .ToList();

            return products.Select(x => _mapper.MapTo(x)).ToArray();
        }
    }
}
