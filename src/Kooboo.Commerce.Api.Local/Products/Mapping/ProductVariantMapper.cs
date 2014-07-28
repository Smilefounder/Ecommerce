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
            var variant = base.Map(source, target, sourceType, targetType, prefix, context) as ProductVariant;
                var fromVariant = source as Kooboo.Commerce.Products.ProductVariant;

            // Variant Fields
            if (context.Includes.Includes(prefix + "VariantFields"))
            {
                foreach (var fieldValue in fromVariant.VariantFields)
                {
                    // TODO: Fix
                    var field = new CustomFieldValue
                    {
                        FieldName = fieldValue.FieldName,
                        //FieldLabel = fieldValue.CustomField.Label,
                        FieldText = fieldValue.FieldValue,
                        FieldValue = fieldValue.FieldValue
                    };

                    //if (fieldValue.CustomField.IsValueLocalizable)
                    //{
                    //    field.FieldText = fromVariant.GetText("VariantFields[" + field.FieldName + "]", context.ApiContext.Culture);
                    //}

                    variant.VariantFields.Add(field);
                }
            }

            // Final price
            // TODO: How to resolve performance issue if it's quering product list with variants?
            var shoppingContext = new Kooboo.Commerce.Carts.ShoppingContext
            {
                Culture = context.ApiContext.Culture.Name,
                Currency = context.ApiContext.Currency
            };

            if (context.ApiContext.CustomerAccountId != null)
            {
                var customer = ((LocalApiContext)context.ApiContext).Services.Customers.GetByAccountId(context.ApiContext.CustomerAccountId);
                if (customer != null)
                {
                    shoppingContext.CustomerId = customer.Id;
                }
            }

            variant.FinalPrice = fromVariant.GetFinalPrice(shoppingContext);

            return variant;
        }
    }
}
