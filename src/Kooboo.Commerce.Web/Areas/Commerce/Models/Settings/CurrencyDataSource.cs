using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{
    public class CurrencyDataSource : ISelectListDataSource
    {
        static List<CurrencyInfo> _currencies;

        static CurrencyDataSource()
        {
            _currencies = CurrencyInfo.GetCurrencies().ToList();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            foreach (var item in _currencies.OrderBy(c => c.ISOSymbol))
            {
                yield return new SelectListItem { Text = item.ISOSymbol, Value = item.ISOSymbol };
            }
        }
    }
}