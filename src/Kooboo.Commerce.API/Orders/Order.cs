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
    public class Order
    {
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

        public PaymentStatus PaymentStatus { get; set; }

        public int? PaymentMethodId { get; set; }

        public string PaymentGatewayName { get; set; }

        public string ExternalPaymentTransactionId { get; set; }

        public DateTime? PaymentCompletedAtUtc { get; set; }

        /// <summary>
        /// Shop can charge fee for different payment methods. 
        /// </summary>
        public decimal PaymentMethodCost { get; set; }

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

        //string ShippingTrackingCode  { get;  set; }

        /// <summary>
        /// Remark from users who ordered it.
        /// </summary>
        public string Remark { get; set; }

        public decimal TotalWeight { get; set; }

        public OrderItem[] OrderItems { get; set; }
        public Customer Customer { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public OrderAddress ShippingAddress { get; set; }
        public OrderAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}