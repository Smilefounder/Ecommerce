using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    [Grid]
    public class ProductGridItemModel
    {
        [GridColumn]
        public int Id { get; set; }

        [LinkColumn("Translate")]
        public string Name { get; set; }

        [GridColumn(HeaderText = "Translated name")]
        public string TranslatedName { get; set; }

        [BooleanColumn(HeaderText = "Translated")]
        public bool IsTranslated { get; set; }

        [BooleanColumn(HeaderText = "Out of date")]
        public bool IsOutOfDate { get; set; }
    }
}