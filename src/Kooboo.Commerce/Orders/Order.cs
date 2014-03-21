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

namespace Kooboo.Commerce.Orders
{
    public class Order
    {
        public Order()
        {
            CreatedAtUtc = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
        }

        [Parameter(Name = "OrderID", DisplayName = "Order ID")]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int? ShoppingCartId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public OrderStatus OrderStatus { get; protected set; }

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

        [Parameter(Name = "OrderTotal", DisplayName = "Order Total")]
        public decimal Total { get; set; }

        /// <summary>
        /// The namer of choosen shipping method. For example, UPS, TNT, DHL, PostMail, etc. 
        /// redundant column
        /// </summary>
        public string ShippingName { get; set; }

        //string ShippingTrackingCode  { get;  set; }

        /// <summary>
        /// Remark from users who ordered it.
        /// </summary>
        public string Remark { get; set; }

        public decimal TotalWeight { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual OrderAddress ShippingAddress { get; set; }
        public virtual OrderAddress BillingAddress { get; set; }

        public virtual void ChangeStatus(OrderStatus newStatus)
        {
            if (OrderStatus != newStatus)
            {
                var oldStatus = OrderStatus;
                OrderStatus = newStatus;
                Event.Apply(new OrderStatusChanged(this, oldStatus, newStatus));
            }
        }
    }
}