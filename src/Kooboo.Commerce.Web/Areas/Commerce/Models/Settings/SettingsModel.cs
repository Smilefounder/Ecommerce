using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{
    public class SettingsModel
    {
        public GlobalSettings Global { get; set; }

        public IList<CustomField> PredefinedFields { get; set; }
    }
}