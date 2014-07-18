using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Products.TopSold
{
    public class TopSoldProductsConfig
    {
        [Description("Top number")]
        public int Num { get; set; }

        [Description("Bought in days")]
        public int Days { get; set; }

        public TopSoldProductsConfig()
        {
            Num = 10;
            Days = 7;
        }
    }
}