using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    [Grid(IdProperty = "Id")]
    public class ProductTypeGridItemModel
    {
        public int Id { get; set; }

        [LinkColumn("Translate")]
        public string Name { get; set; }
    }
}