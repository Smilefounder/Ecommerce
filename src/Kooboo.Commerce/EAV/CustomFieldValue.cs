using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV
{
    public class CustomFieldValue
    {
        public int FieldId { get; set; }

        public string FieldValue { get; set; }

        public CustomFieldValue(int fieldId, string value)
        {
            FieldId = fieldId;
            FieldValue = value;
        }
    }
}
