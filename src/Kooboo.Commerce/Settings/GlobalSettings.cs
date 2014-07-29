using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Settings
{
    public class GlobalSettings
    {
        /// <summary>
        /// The ISO currency code, EUR, USD, CNY, etc. 
        /// </summary>
        [StringLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Weight unit name, eg, pound, kg. 
        /// </summary>
        public string WeightUnitName { get; set; }

        /// <summary>
        /// Size unit name, e.g. cm, inch, etc. 
        /// </summary>
        public string SizeUnitName { get; set; }

        public ImageSettings Image { get; set; }

        public GlobalSettings()
        {
            Currency = "USD";
            WeightUnitName = "KG";
            SizeUnitName = "MM";
            Image = new ImageSettings();
        }
    }
}
