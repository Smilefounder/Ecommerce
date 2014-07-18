using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.API.Customers;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// order id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// customer id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// shopping cart id, can be null
        /// </summary>
        public int? ShoppingCartId { get; set; }
        /// <summary>
        /// shipping address id
        /// when create order from a shopping cart, the shipping address is copied from shopping cart's shipping address
        /// </summary>
        public int? ShippingAddressId { get; set; }
        /// <summary>
        /// billing address id
        /// when create order from a shopping cart, the billing address is copied from shopping cart's billing address
        /// </summary>
        public int? BillingAddressId { get; set; }
        /// <summary>
        /// order created time
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>
        /// order status
        /// </summary>
        public OrderStatus Status { get; set; }

        public string ProcessingStatus { get; set; }

        /// <summary>
        /// is this order completed
        /// </summary>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// use coupon to this order for payment
        /// </summary>
        public string Coupon { get; set; }
        /// <summary>
        /// the subtotal of the order items 
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// discount to the subtotal
        /// discount comes from many ways, such as: coupon, promotion, custom loyalty etc.
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// toal tax of the order
        /// </summary>
        public decimal TotalTax { get; set; }
        /// <summary>
        /// shipment cost
        /// </summary>
        public decimal ShippingCost { get; set; }

        /// <summary>
        /// Shop can charge fee for different payment methods. 
        /// </summary>
        public decimal PaymentMethodCost { get; set; }
        /// <summary>
        /// total = subtotal - discount + total tax + shipping cost + payment cost
        /// the value that user should pay
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// The namer of choosen shipping method. For example, UPS, TNT, DHL, PostMail, etc. 
        /// redundant column
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// redundant column
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// Remark from users who ordered it.
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// total weight of the order items
        /// this can be used to calculate the shipping cost
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// order items
        /// </summary>
        public OrderItem[] OrderItems { get; set; }
        /// <summary>
        /// order customer
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// shipping address
        /// </summary>
        public OrderAddress ShippingAddress { get; set; }
        /// <summary>
        /// billing address
        /// </summary>
        public OrderAddress BillingAddress { get; set; }
        /// <summary>
        /// payment method
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
    }
}