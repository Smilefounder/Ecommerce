using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Framework.UI.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders
{
    public class OrderModel : IOrderModel
    {
        [LinkColumn("Detail")]
        public int Id { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        [GridColumn(HeaderText = "Customer")]
        public string CustomerFullName
        {
            get
            {
                return CustomerFirstName + " " + CustomerLastName;
            }
        }

        [GridColumn]
        public decimal Total { get; set; }

        [GridColumn(HeaderText = "Order status")]
        public OrderStatus Status { get; set; }

        [GridColumn(HeaderText = "Processing status")]
        public string ProcessingStatus { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [GridColumn(HeaderText = "Creation date")]
        public DateTime CreatedAt
        {
            get
            {
                return CreatedAtUtc.ToLocalTime();
            }
        }

        public OrderModel()
        {
        }

        public OrderModel(Order order, Customer customer)
        {
            Id = order.Id;
            CustomerFirstName = customer.FirstName;
            CustomerLastName = customer.LastName;
            Total = order.Total;
            CreatedAtUtc = order.CreatedAtUtc;
            Status = order.Status;
            ProcessingStatus = order.ProcessingStatus;
        }
    }
}