using Kooboo.Commerce.Products;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Activities;
using System.ComponentModel;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityVisible("Product Events")]
    public interface IProductEvent : IEvent
    {
        int ProductId { get; }
    }

    [ActivityVisible("Product Price Events")]
    public interface IProdutPriceEvent : IProductEvent
    {
        int ProductPriceId { get; }
    }
}
