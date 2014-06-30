using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount
{
    public class RetailPriceDiscountActivity : ActivityBase<GetPrice>, IHasCustomActivityConfigEditor
    {
        public override string Name
        {
            get
            {
                return "RetailPriceDiscount";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Apply discount to product retail price";
            }
        }

        public override Type ConfigModelType
        {
            get
            {
                return typeof(RetailPriceDiscountActivityConfig);
            }
        }

        protected override void Execute(GetPrice @event, ActivityContext context)
        {
            var config = context.Config as RetailPriceDiscountActivityConfig;
            if (config == null)
            {
                return;
            }

            var newPrice = config.ApplyDiscount(@event.FinalUnitPrice);
            @event.FinalUnitPrice = newPrice;
        }

        public string GetEditorVirtualPath(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return String.Format("~/Areas/{0}/Views/Config.cshtml", Strings.AreaName);
        }
    }
}