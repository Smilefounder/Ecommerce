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
    [EventCategory("Product Events")]
    public interface IProductEvent : IDomainEvent
    {
        Product Product { get; }
    }

    public interface IProdutPriceEvent : IProductEvent
    {
        ProductPrice ProductPrice { get; }
    }
}
