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

        [LinkedGridColumn(TargetAction = "BasicInfo", HeaderText = "Display name")]
        public string DisplayName { get; set; }

        [GridColumn(HeaderText = "Payment processor")]
        public string PaymentProcessorName { get; set; }

        [GridColumn(HeaderText = "Additional fee")]
        public string AdditionalFee { get; set; }

        [BooleanGridColumn(HeaderText = "Enabled")]
        public bool IsEnabled { get; set; }

        public bool IsConfigurable { get; set; }

        public DateTime CreatedAt { get; set; }

        public PaymentMethodRowModel() { }

        public PaymentMethodRowModel(PaymentMethod method)
        {
            Id = method.Id;
            DisplayName = method.Name;
            PaymentProcessorName = method.PaymentProcessorName;
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