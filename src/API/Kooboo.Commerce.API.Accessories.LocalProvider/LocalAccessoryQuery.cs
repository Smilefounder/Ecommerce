using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Accessories;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.LocalProvider;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Products.Services;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Commerce.API.Accessories.LocalProvider
{
    [Dependency(typeof(IAccessoryQuery))]
    public class LocalAccessoryQuery : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IAccessoryQuery
    {
        private IProductService _productService;
        private IProductAccessoryService _accessoryService;

        public LocalAccessoryQuery(
            IHalWrapper halWrapper,
            IProductService productService,
            IProductAccessoryService accessoryService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
            : base(halWrapper, mapper)
        {
            _productService = productService;
            _accessoryService = accessoryService;
        }

        protected override IQueryable<Commerce.Products.Product> CreateQuery()
        {
            return _productService.Query();
        }

        protected override IQueryable<Commerce.Products.Product> OrderByDefault(IQueryable<Commerce.Products.Product> query)
        {
            return query;
        }

        public IAccessoryQuery ByProduct(int productId)
        {
            EnsureQuery();

            var accessories = _accessoryService.GetAccessories(productId);
            var accessoryIds = accessories.Select(x => x.ProductId).ToArray();
            _query = _query.Where(x => accessoryIds.Contains(x.Id));

            return this;
        }
    }
}
