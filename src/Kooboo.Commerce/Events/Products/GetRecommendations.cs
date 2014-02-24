using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class GetRecommendations : IProductEvent, IQueryProductListEvent
    {
        public Product Product { get; set; }

        public IEnumerable<Product> Result { get; set; }
    }
}
