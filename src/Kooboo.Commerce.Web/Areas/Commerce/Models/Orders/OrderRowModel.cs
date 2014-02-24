using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Orders
{
    [Grid(IdProperty = "Id", Checkable = true)]
    public class OrderRowModel
    {
        [GridColumn]
        public int Id { get; set; }

        [GridColumn]
        public Customer Customer { get; set; }

        [GridColumn]
        public decimal Total { get; set; }

        [GridColumn(HeaderText = "Created date")]
        public DateTime CreatedAt { get; set; }

        [GridColumn(HeaderText = "Order status")]
        public OrderStatus OrderStatus { get; set; }

        [GridColumn(HeaderText = "Payment status")]
        public PaymentStatus PaymentStatus { get; set; }

        public OrderRowModel()
        {
        }

        public OrderRowModel(Order order, Customer customer)
        {
            Id = order.Id;
            Customer = customer;
            Total = order.Total;
            CreatedAt = order.CreatedAtUtc.ToLocalTime();
            OrderStatus = order.OrderStatus;
            PaymentStatus = order.PaymentStatus;
        }
    }
}