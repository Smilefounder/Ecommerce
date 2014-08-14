using Kooboo.Commerce.Api.Local.Mapping;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Web.Framework.UI.Form;

namespace Kooboo.Commerce.Api.Local.Products.Mapping
{
    public class ProductMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            var services = (context.ApiContext as LocalApiContext).Services;

            var model = base.Map(source, target, sourceType, targetType, prefix, context) as Product;
            var product = source as Kooboo.Commerce.Products.Product;
            var productType = services.ProductTypes.GetById(product.ProductTypeId);

            model.SkuAlias = productType.SkuAlias;

            model.LowestPrice = product.Variants.Min(v => v.Price);
            model.HighestPrice = product.Variants.Max(v => v.Price);

            return model;
        }

        protected override void MapProperty(System.Reflection.PropertyInfo targetProperty, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            var model = target as Product;

            if (targetProperty.Name == "Categories" && context.Includes.Includes(propertyPath))
            {
                var fromProduct = source as Kooboo.Commerce.Products.Product;

                foreach (var fromCategory in fromProduct.Categories)
                {
                    var mapper = GetMapperOrDefault(typeof(Kooboo.Commerce.Categories.Category), typeof(Category));
                    var category = mapper.Map(fromCategory, new Category(), typeof(Kooboo.Commerce.Categories.Category), typeof(Category), null, new MappingContext(context.ApiContext)) as Category;
                    model.Categories.Add(category);
                }
            }
            else if (targetProperty.Name == "CustomFields" && context.Includes.Includes(propertyPath))
            {
                var services = (context.ApiContext as LocalApiContext).Services;

                var product = source as Kooboo.Commerce.Products.Product;
                var productType = services.ProductTypes.GetById(product.ProductTypeId);

                var controls = FormControls.Controls().ToList();

                foreach (var definition in productType.CustomFieldDefinitions)
                {
                    var field = product.CustomFields.FirstOrDefault(f => f.FieldName == definition.Name);
                    var fieldModel = new CustomField
                    {
                        FieldName = definition.Name,
                        FieldLabel = productType.GetText("CustomFieldDefinitions[" + definition.Name + "].Label", context.ApiContext.Culture),
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
                            fieldModel.FieldText = product.GetText("CustomFields[" + field.FieldName + "]", context.ApiContext.Culture);
                        }
                        else
                        {
                            if (control.IsSelectionList)
                            {
                                fieldModel.FieldText = productType.GetText("CustomFieldDefinitions[" + definition.Name + "].SelectionItems[" + field.FieldValue + "]", context.ApiContext.Culture);
                            }
                            else
                            {
                                fieldModel.FieldText = productType.GetText("CustomFieldDefinitions[" + definition.Name + "].DefaultValue", context.ApiContext.Culture);
                            }
                        }
                    }

                    if (String.IsNullOrEmpty(fieldModel.FieldText))
                    {
                        fieldModel.FieldText = fieldModel.FieldValue;
                    }

                    model.CustomFields.Add(fieldModel);
                }
            }
            else
            {
                base.MapProperty(targetProperty, source, target, sourceType, targetType, propertyPath, context);
            }
        }
    }
}
