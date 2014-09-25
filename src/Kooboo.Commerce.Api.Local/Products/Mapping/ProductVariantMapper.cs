using Kooboo.Commerce.Api.Local.Mapping;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Web.Framework.UI.Form;
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
            var model = base.Map(source, target, sourceType, targetType, prefix, context) as ProductVariant;
            var variant = source as Kooboo.Commerce.Products.ProductVariant;

            // Final price
            var shoppingContext = new Kooboo.Commerce.Carts.ShoppingContext
            {
                Culture = context.ApiContext.Culture.Name,
                Currency = context.ApiContext.Currency
            };

            if (context.ApiContext.CustomerEmail != null)
            {
                var service = new Kooboo.Commerce.Customers.CustomerService(context.ApiContext.Database);
                var customer = service.FindByEmail(context.ApiContext.CustomerEmail);
                if (customer != null)
                {
                    shoppingContext.CustomerId = customer.Id;
                }
            }

            model.FinalPrice = variant.GetFinalPrice(shoppingContext);

            return model;
        }

        protected override void MapProperty(System.Reflection.PropertyInfo targetProperty, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            var model = target as ProductVariant;
            var variant = source as Kooboo.Commerce.Products.ProductVariant;

            if (targetProperty.Name == "VariantFields" && context.Includes.Includes(propertyPath))
            {
                var product = context.ApiContext.Database.Repository<Kooboo.Commerce.Products.Product>().Find(variant.ProductId);
                var productType = product.ProductType;
                
                var controls = FormControls.Controls().ToList();

                foreach (var definition in productType.VariantFieldDefinitions)
                {
                    var field = variant.VariantFields.FirstOrDefault(f => f.FieldName == definition.Name);
                    var fieldModel = new CustomField
                    {
                        FieldName = definition.Name,
                        FieldLabel = productType.GetText("VariantFieldDefinitions[" + definition.Name + "].Label", context.ApiContext.Culture),
                        FieldValue = field == null ? null : field.FieldValue
                    };

                    if (String.IsNullOrEmpty(fieldModel.FieldLabel))
                    {
                        fieldModel.FieldLabel = definition.Label;
                    }

                    if (field != null)
                    {
                        var control = controls.Find(c => c.Name == definition.ControlType);
                        if (!control.IsSelectionList && !control.IsValuesPredefined)
                        {
                            fieldModel.FieldText = variant.GetText("VariantFields[" + definition.Name + "]", context.ApiContext.Culture);
                        }
                        else
                        {
                            if (control.IsSelectionList)
                            {
                                fieldModel.FieldText = productType.GetText("VariantFieldDefinitions[" + definition.Name + "].SelectionItems[" + fieldModel.FieldValue + "]", context.ApiContext.Culture);
                            }
                            else
                            {
                                fieldModel.FieldText = productType.GetText("VariantFieldDefinitions[" + definition.Name + "].DefaultValue", context.ApiContext.Culture);
                            }
                        }
                    }

                    if (String.IsNullOrEmpty(fieldModel.FieldText))
                    {
                        fieldModel.FieldText = fieldModel.FieldValue;
                    }

                    model.VariantFields.Add(fieldModel);
                }
            }
            else
            {
                base.MapProperty(targetProperty, source, target, sourceType, targetType, propertyPath, context);
            }
        }
    }
}
