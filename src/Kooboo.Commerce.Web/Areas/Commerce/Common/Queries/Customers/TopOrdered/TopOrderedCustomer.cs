using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Customers.TopOrdered
{
    public class TopOrderedCustomer : CustomerModel
    {
        [GridColumn(Order = 10)]
        public int OrdersCount { get; set; }
    }
}