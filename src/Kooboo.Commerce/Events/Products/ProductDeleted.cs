﻿using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 500)]
    public class ProductDeleted : BusinessEvent, IProductEvent
    {
        [Param]
        public int ProductId { get; set; }

        [Param]
        public string ProductName { get; set; }

        protected ProductDeleted() { }

        public ProductDeleted(Product product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
        }
    }
}
