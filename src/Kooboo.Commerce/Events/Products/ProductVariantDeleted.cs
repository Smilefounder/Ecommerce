﻿using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class ProductVariantDeleted : Event, IProductEvent
    {
        [Param]
        public int ProductId { get; set; }

        [Param]
        public int ProductPriceId { get; set; }

        protected ProductVariantDeleted() { }

        public ProductVariantDeleted(Product product, ProductPrice price)
        {
            ProductId = product.Id;
            ProductPriceId = price.Id;
        }
    }
}
