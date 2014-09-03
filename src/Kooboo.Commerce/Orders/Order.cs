using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
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
        public OrderStatus Status { get; set; }

        [Param]
        [StringLength(50)]
        public string ProcessingStatus { get; set; }

        [StringLength(50)]
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

        [StringLength(500)]
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

        public void SetCustomField(string name, string value)
        {
            var field = CustomFields.FirstOrDefault(f => f.Name == name);
            if (field != null)
            {
                field.Value = value;
            }
            else
            {
                field = new OrderCustomField(name, value);
                CustomFields.Add(field);
            }
        }
    }
}