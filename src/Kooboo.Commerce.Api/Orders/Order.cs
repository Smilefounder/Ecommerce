using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Customers;

namespace Kooboo.Commerce.Api.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int? ShoppingCartId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public OrderStatus Status { get; set; }

        public string ProcessingStatus { get; set; }

        public string Coupon { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal ShippingCost { get; set; }

        public int? ShippingMethodId { get; set; }

        public string ShippingMethodName { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public decimal Total { get; set; }

        public decimal TotalPaid { get; set; }

        public string Remark { get; set; }

        public decimal TotalWeight { get; set; }
        
        [OptionalInclude]
        public IList<OrderItem> OrderItems { get; set; }
        
        [OptionalInclude]
        public Customer Customer { get; set; }
        
        [OptionalInclude]
        public OrderAddress ShippingAddress { get; set; }
        
        [OptionalInclude]
        public OrderAddress BillingAddress { get; set; }

        [OptionalInclude]
        public IDictionary<string, string> CustomFields { get; set; }
    }
}