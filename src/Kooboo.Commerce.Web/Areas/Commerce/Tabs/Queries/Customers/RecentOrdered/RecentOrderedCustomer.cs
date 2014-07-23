using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers.RecentOrdered
{
    public class RecentOrderedCustomer : CustomerModel
    {
        [GridColumn(Order = 10)]
        public int OrdersCount { get; set; }

        public RecentOrderedCustomer() { }

        public RecentOrderedCustomer(Customer customer)
            : base(customer)
        {
        }
    }
}