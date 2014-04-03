using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductDto = Kooboo.Commerce.API.Products.Product;

namespace Kooboo.Commerce.Products.Accessories.Api
{
    public class AccessoriesController : ApiController
    {
        private ISettingService _settingService;
        private IProductService _productService;

        public AccessoriesController(
            ISettingService settingService,
            IProductService productService)
        {
            _settingService = settingService;
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<ProductDto> Get(int productId)
        {
            var service = new ProductAccessoryService(_settingService);
            var accessories = service.GetAccessories(productId).ToArray();
            if (accessories.Length == 0)
            {
                return Enumerable.Empty<ProductDto>();
            }

            var productIds = accessories.Select(x => x.ProductId).ToArray();

            var prices = _productService.Query()
                                        .Where(x => x.IsPublished && productIds.Contains(x.Id))
                                        .ToList();

            var dtos = new List<ProductDto>();

            foreach (var price in prices)
            {
                dtos.Add(new ProductDto
                {
                    Id = price.Id,
                    Name = price.Name
                });
            }

            return dtos;
        }
    }
}
