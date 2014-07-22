using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions.Grid2;
using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(PromotionRowModelGridItem))]
    public class PromotionRowModel
    {
        public int Id { get; set; }

        [LinkColumn("BasicInfo")]
        public string Name { get; set; }

        [GridColumn(HeaderText = "Coupon code")]
        public string CouponCode { get; set; }

        [GridColumn]
        public int Priority { get; set; }

        [BooleanColumn(HeaderText = "Enabled")]
        public bool IsEnabled { get; set; }

        public bool IsSelected { get; set; }

        public PromotionRowModel()
        {
        }

        public PromotionRowModel(Promotion promotion)
        {
            Id = promotion.Id;
            Name = promotion.Name;
            IsEnabled = promotion.IsEnabled;
            Priority = promotion.Priority;

            if (promotion.RequireCouponCode)
            {
                CouponCode = promotion.CouponCode;
            }
            else
            {
                CouponCode = "-";
            }
        }
    }
}