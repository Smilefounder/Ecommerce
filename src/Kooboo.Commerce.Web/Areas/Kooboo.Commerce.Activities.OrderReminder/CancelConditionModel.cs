using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.OrderReminder
{
    public class CancelConditionModel
    {
        [Param]
        public OrderStatus OrderStatus { get; set; }
    }
}