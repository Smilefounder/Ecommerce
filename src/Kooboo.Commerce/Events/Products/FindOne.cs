using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class FindOne : Event, IProductEvent
    {
        public Product Product { get; set; }

        public FindOne(Product product)
        {
            Product = product;
        }
    }
}
