using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Rules.Activities.ProductPriceDiscount
{
    public class RetailPriceDiscountActivity : Activity<GetPrice>, IHasCustomActivityConfigEditor
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

        public override Type ConfigType
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

            var newPrice = config.ApplyDiscount(@event.FinalPrice);
            @event.FinalPrice = newPrice;
        }

        public string GetEditorVirtualPath()
        {
            return String.Format("~/Areas/{0}/Views/Config.cshtml", Strings.AreaName);
        }
    }
}