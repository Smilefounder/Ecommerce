using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers.RecentOrdered
{
    public class RecentOrderedCustomersConfig
    {
        [Description("Customer ordered days before")]
        public int Days { get; set; }

        public RecentOrderedCustomersConfig()
        {
            Days = 7;
        }
    }
}