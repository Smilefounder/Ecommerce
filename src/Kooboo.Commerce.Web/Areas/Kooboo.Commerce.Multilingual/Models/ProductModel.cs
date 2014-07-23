using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CustomFieldValueModel> CustomFields { get; set; }

        public ProductModel()
        {
            CustomFields = new List<CustomFieldValueModel>();
        }
    }
}