using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Orders
{
    [Grid]
    public class OrderEditorModel
    {
        public int Id { get; set; }

        public void UpdateTo(Order order)
        {
        }
    }
}