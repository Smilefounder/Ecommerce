using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class GetProductActions : Event
    {
        [Reference(typeof(Product))]
        public int ProductId { get; private set; }

        public IList<string> ActionNames { get; private set; }

        public GetProductActions(int productId)
        {
            ProductId = productId;
            ActionNames = new List<string>();
        }
    }
}
