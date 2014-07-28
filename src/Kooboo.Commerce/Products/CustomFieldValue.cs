using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV
{
    public class CustomFieldValue
    {
        public CustomField Field { get; set; }

        public string FieldValue { get; set; }

        public CustomFieldValue(CustomField field, string value)
        {
            Field = field;
            FieldValue = value;
        }
    }
}
