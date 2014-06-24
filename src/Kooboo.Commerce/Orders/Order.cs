using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Orders
{
    public class Order
    {
        public Order()
        {
            CreatedAtUtc = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
            CustomFields = new List<OrderCustomField>();
        }

        [Param]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int? ShoppingCartId { get; set; }

        public int? ShippingAddressId { get; set; }

        public int? BillingAddressId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [Param]
        public OrderStatus OrderStatus { get; set; }

        public string Coupon { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal ShippingCost { get; set; }

        public int? ShippingMethodId { get; set; }

        [StringLength(50)]
        public string ShippingMethodName { get; set; }

        /// <summary>
        /// Shop can charge fee for different payment methods. 
        /// </summary>
        public decimal PaymentMethodCost { get; set; }

        [Param]
        public decimal Total { get; set; }

        /// <summary>
        /// Amount already paid by the customer. An order is marked as Paid when TotalPaid equals Total.
        /// </summary>
        public decimal TotalPaid { get; set; }

        /// <summary>
        /// Remark from users who ordered it.
        /// </summary>
        public string Remark { get; set; }

        // TODO: How to calculate?
        public decimal TotalWeight { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        [Reference]
        public virtual Customer Customer { get; set; }

        [Reference]
        public virtual OrderAddress ShippingAddress { get; set; }

        [Reference]
        public virtual OrderAddress BillingAddress { get; set; }

        public virtual ICollection<OrderCustomField> CustomFields { get; set; }
    }
}