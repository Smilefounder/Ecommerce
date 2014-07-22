using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    [MetadataFor(typeof(Brand))]
    [Grid(Checkable = true, IdProperty = "Id")]
    public class Brand_Metadata
    {
        [LinkColumn("Translate")]
        public string Name { get; set; }

        [GridColumn]
        public string Description { get; set; }
    }
}