using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods.Grid2;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(PaymentMethodRowModelGridItem))]
    public class PaymentMethodRowModel
    {
        public int Id { get; set; }

        [EditorLinkedGridColumn]
        public string DisplayName { get; set; }

        public string PaymentGatewayName { get; set; }

        [GridColumn]
        public string AdditionalFee { get; set; }

        [BooleanGridColumn]
        public bool IsEnabled { get; set; }

        public bool IsConfigurable { get; set; }

        public DateTime CreatedAt { get; set; }

        public PaymentMethodRowModel() { }

        public PaymentMethodRowModel(PaymentMethod method)
        {
            Id = method.Id;
            DisplayName = method.DisplayName;
            PaymentGatewayName = method.PaymentGatewayName;
            IsEnabled = method.IsEnabled;
            CreatedAt = method.CreatedAtUtc.ToLocalTime();

            if (method.AdditionalFeeChargeMode == PriceChangeMode.ByPercent)
            {
                AdditionalFee = method.AdditionalFeeAmount.ToString("f2") + "%";
            }
            else
            {
                AdditionalFee = method.AdditionalFeeAmount.ToString("f2");
            }
        }
    }
}