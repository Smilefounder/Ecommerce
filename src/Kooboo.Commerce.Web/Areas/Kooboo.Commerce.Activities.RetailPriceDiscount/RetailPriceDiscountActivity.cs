using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount
{
    [Dependency(typeof(IActivity), Key = "RetailPriceDiscount")]
    public class RetailPriceDiscountActivity : IActivity
    {
        public string Name
        {
            get
            {
                return "RetailPriceDiscount";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Apply discount to product retail price";
            }
        }

        public bool AllowAsyncExecution
        {
            get
            {
                return false;
            }
        }

        public bool CanBindTo(Type eventType)
        {
            return eventType == typeof(GetPrice);
        }

        public void Execute(Commerce.Events.IEvent @event, ActivityContext context)
        {
            var config = context.GetActivityConfig<RetailPriceDiscountActivityConfig>();
            if (config == null)
            {
                return;
            }

            var eventInfo = @event as GetPrice;
            var newPrice = config.ApplyDiscount(eventInfo.FinalPrice);

            eventInfo.FinalPrice = newPrice;
        }

        public ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return new ActivityEditor(String.Format("~/Areas/{0}/Views/Config.cshtml", Strings.AreaName));
        }
    }
}