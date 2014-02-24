using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.EAV
{
    public class CustomFieldsModel
    {
        public string OwnerType { get; set; }

        public string OwnerKey { get; set; }

        public string FieldType { get; set; }

        public List<CustomField> Fields { get; set; }

        public CustomFieldsModel()
        {
            FieldType = CustomFieldTypes.Default;
        }
    }
}