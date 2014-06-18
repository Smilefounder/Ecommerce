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
        /// The culture shown on your website. This is used to display currency, date, number and other culture related content.
        /// </summary>
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
            Culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            CurrencyISOCode = "USD";
            WeightUnitName = "KG";
            SizeUnitName = "MM";
        }
    }
}
