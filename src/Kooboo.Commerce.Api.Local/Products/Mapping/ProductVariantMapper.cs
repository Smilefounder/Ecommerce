using Kooboo.Commerce.Api.Local.Mapping;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Products.Mapping
{
    public class ProductVariantMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            var variant = base.Map(source, target, sourceType, targetType, prefix, context) as ProductPrice;

            // Variant Fields
            if (context.Includes.Includes(prefix + "VariantFields"))
            {
                var fromVariant = source as Kooboo.Commerce.Products.ProductPrice;
                foreach (var fieldValue in fromVariant.VariantValues)
                {
                    var field = new CustomFieldValue
                    {
                        FieldName = fieldValue.CustomField.Name,
                        FieldLabel = fieldValue.CustomField.Label,
                        FieldText = fieldValue.FieldValue,
                        FieldValue = fieldValue.FieldValue
                    };

                    if (fieldValue.CustomField.IsValueLocalizable)
                    {
                        field.FieldText = fromVariant.GetText("VariantFields[" + field.FieldName + "]", context.Culture);
                    }

                    variant.VariantFields.Add(field);
                }
            }

            return variant;
        }
    }
}
