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

        public PaymentStatus PaymentStatus { get; protected set; }

        public int? PaymentMethodId { get; set; }

        public string PaymentGatewayName { get; set; }

        public string ExternalPaymentTransactionId { get; set; }

        public DateTime? PaymentCompletedAtUtc { get; set; }

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

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual OrderAddress ShippingAddress { get; set; }
        public virtual OrderAddress BillingAddress { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual void MarkPaymentSucceeded(string paymentTransactionId)
        {
            if (PaymentStatus != PaymentStatus.Success)
            {
                var oldStatus = PaymentStatus;
                PaymentStatus = Payments.PaymentStatus.Success;
                PaymentCompletedAtUtc = DateTime.UtcNow;
                ExternalPaymentTransactionId = paymentTransactionId;

                Event.Apply(new PaymentSucceeded(this, oldStatus));

                ChangeOrderStatus(OrderStatus.Paid);
            }
        }

        public virtual void MarkPaymentCancelled()
        {
            if (PaymentStatus != Payments.PaymentStatus.Cancelled)
            {
                var oldStatus = PaymentStatus;
                PaymentStatus = Payments.PaymentStatus.Cancelled;
                Event.Apply(new PaymentCancelled(this, oldStatus));
            }
        }

        public virtual void MarkPaymentFailed()
        {
            if (PaymentStatus != Payments.PaymentStatus.Failed)
            {
                var oldStatus = PaymentStatus;
                PaymentStatus = Payments.PaymentStatus.Failed;
                Event.Apply(new PaymentFailed(this, oldStatus));
            }
        }

        /// <summary>
        /// 用于管理员在后台直接强制更改支付状态，这个方法没有真实的支付动作发生。
        /// </summary>
        public virtual void ForceChangePaymentStatus(PaymentStatus newPaymentStatus)
        {
            if (PaymentStatus != newPaymentStatus)
            {
                var oldStatus = PaymentStatus;

                PaymentStatus = newPaymentStatus;

                if (newPaymentStatus == Payments.PaymentStatus.Success)
                {
                    PaymentCompletedAtUtc = DateTime.UtcNow;
                }

                Event.Apply(new PaymentStatusChanged(this, oldStatus, newPaymentStatus));
            }
        }

        public virtual void ChangeOrderStatus(OrderStatus newStatus)
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