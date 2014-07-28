using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class CustomFieldValueModel
    {
        public string FieldName { get; set; }

        public string FieldLabel { get; set; }

        public string ControlType { get; set; }

        public string FieldValue { get; set; }

        public CustomField Field { get; set; }
    }
}