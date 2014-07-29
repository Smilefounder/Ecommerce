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
        static IEnumerable<CurrencyInfo> _currencies;

        static CurrencyDataSource()
        {
            _currencies = GetCurrencies();
        }

        static IEnumerable<CurrencyInfo> GetCurrencies()
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var list = new List<CurrencyInfo>();
            //loop through all the cultures found
            foreach (CultureInfo culture in cultures)
            {
                //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                //to the RegionInfo contructor to gain access to the information for that culture
                try
                {
                    RegionInfo region = new RegionInfo(culture.LCID);
                    if (list.Any(i => i.EnglishName == region.CurrencyEnglishName) == false)
                    {
                        list.Add(new CurrencyInfo
                        {
                            EnglishName = region.CurrencyEnglishName,
                            NativeName = region.CurrencyNativeName,
                            Symbol = region.CurrencySymbol,
                            ISOSymbol = region.ISOCurrencySymbol
                        });
                    }
                }
                catch (System.Exception)
                {
                    //next 
                }
            }

            return list;
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