using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Actions.Products
{
    public class ProductActionsRegistration : IHandle<GetProductActions>
    {
        private IProductService _productService;

        public ProductActionsRegistration(IProductService productService)
        {
            _productService = productService;
        }

        public void Handle(GetProductActions @event)
        {
            var product = _productService.GetById(@event.ProductId);
            if (product.IsPublished)
            {
                @event.ActionNames.Add("UnpublishProduct");
            }
            else
            {
                @event.ActionNames.Add("PublishProduct");
            }
        }
    }
}