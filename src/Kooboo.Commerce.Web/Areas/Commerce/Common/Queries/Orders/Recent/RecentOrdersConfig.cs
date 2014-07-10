using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Orders.Recent
{
    public class RecentOrdersConfig
    {
        [Description("Ordered days before")]
        public int Days { get; set; }

        public RecentOrdersConfig()
        {
            Days = 7;
        }
    }
}