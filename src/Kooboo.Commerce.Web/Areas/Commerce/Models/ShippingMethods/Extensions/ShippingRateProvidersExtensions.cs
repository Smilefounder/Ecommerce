using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods
{
    public static class ShippingRateProvidersExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<IShippingRateProvider> providers)
        {
            foreach (var provider in providers)
            {
                yield return new SelectListItem
                {
                    Text = provider.Name,
                    Value = provider.Name
                };
            }
        }
    }
}