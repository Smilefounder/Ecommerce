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

namespace Kooboo.Commerce.Orders
{
    public class Order : INotifyCreated
    {
        public Order()
        {
            CreatedAtUtc = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
        }

        [Param]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int? ShoppingCartId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool IsCompleted { get; set; }

        public string Coupon { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalTax { get; set; }

        public decimal ShippingCost { get; set; }

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
        /// The namer of choosen shipping method. For example, UPS, TNT, DHL, PostMail, etc. 
        /// redundant column
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// Remark from users who ordered it.
        /// </summary>
        public string Remark { get; set; }

        public decimal TotalWeight { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        [Reference]
        public virtual Customer Customer { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        [Reference]
        public virtual OrderAddress ShippingAddress { get; set; }

        [Reference]
        public virtual OrderAddress BillingAddress { get; set; }

        public virtual ICollection<OrderCustomField> CustomFields { get; set; }

        void INotifyCreated.NotifyCreated()
        {
            Event.Raise(new OrderCreated(this));
        }
    }
}