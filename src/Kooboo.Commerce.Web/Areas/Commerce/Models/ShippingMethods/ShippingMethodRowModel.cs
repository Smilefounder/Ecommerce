using Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods.Grid2;
using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(ShippingMethodRowModelGridItem))]
    public class ShippingMethodRowModel
    {
        public int Id { get; set; }

        [LinkColumn("BasicInfo")]
        public string Name { get; set; }

        [GridColumn(HeaderText = "Shipping rate provider")]
        public string ShippingRateProviderName { get; set; }

        [BooleanColumn(HeaderText = "Enabled")]
        public bool IsEnabled { get; set; }
    }
}