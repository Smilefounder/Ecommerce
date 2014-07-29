using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public static class CustomFieldExtensions
    {
        public static string GetValue(this IEnumerable<ProductCustomField> fieldValues, string fieldName)
        {
            var fieldValue = fieldValues.FirstOrDefault(f => f.FieldName == fieldName);
            return fieldValue == null ? null : fieldValue.FieldValue;
        }
    }
}
