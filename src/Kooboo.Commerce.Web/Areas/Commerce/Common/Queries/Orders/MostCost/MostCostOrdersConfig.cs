using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Orders.MostCost
{
    public class MostCostOrdersConfig
    {
        [Description("Top number")]
        public int Num { get; set; }

        [Description("The least total price in the order")]
        public decimal TotalPrice { get; set; }

        public MostCostOrdersConfig()
        {
            Num = 10;
        }
    }
}