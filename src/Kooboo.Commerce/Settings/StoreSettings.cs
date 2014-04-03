using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Settings
{
    public class StoreSettings
    {
        public static readonly string Key = typeof(StoreSettings).Name;

        /// <summary>
        /// The price index to display all prices. Can be used:
        /// 1. To increase or descrease price of all items. 
        /// 2. Use to convert into different currency when used in different shops. 
        /// </summary>
        public decimal PriceIndex { get; set; }

        /// <summary>
        /// The culture shown on your website. This is used to display currency, date, number and other culture related content.
        /// </summary>
        /// // only value is stored in database. The culture should be generated from .NET list.
        //public  IEnumerable<SelectListItem> Culture { get; set; }
        public string Culture { get; set; }

        /// <summary>
        /// The ISO currency code, EUR, USD, CNY, etc. 
        /// </summary>
        [StringLength(3)]
        public string CurrencyISOCode { get; set; }

        /// <summary>
        /// Weight unit name, eg, pound, kg. 
        /// </summary>
        public string WeightUnitName { get; set; }

        /// <summary>
        /// Size unit name, e.g. cm, inch, etc. 
        /// </summary>
        public string SizeUnitName { get; set; }

        public StoreSettings()
        {
            PriceIndex = 1.000M;
            Culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            CurrencyISOCode = "USD";
            WeightUnitName = "KG";
            SizeUnitName = "MM";
        }
    }
}
