using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Orders.Default
{
    public class DefaultOrdersQueryConfig
    {
        [UIHint("DropDownList")]
        [DataSource(typeof(OrderStatusDataSource))]
        public OrderStatus? Status { get; set; }

        [Display(Name = "Processing status")]
        public string ProcessingStatus { get; set; }
    }
}