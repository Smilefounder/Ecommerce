using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Brands
{
    public class BrandEditorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public List<NameValue> CustomFields { get; set; }

        public BrandEditorModel()
        {
            CustomFields = new List<NameValue>();
        }
    }
}