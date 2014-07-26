using Kooboo.Commerce.Api.Local.Mapping;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Products.Mapping
{
    public class ProductMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            var product = base.Map(source, target, sourceType, targetType, prefix, context) as Product;
            var fromProduct = source as Kooboo.Commerce.Products.Product;

            product.SkuAlias = fromProduct.Type.SkuAlias;

            if (context.Includes.Includes(prefix + "CustomFields"))
            {
                foreach (var fieldValue in fromProduct.CustomFieldValues)
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
                        field.FieldText = fromProduct.GetText("CustomFields[" + field.FieldName + "]", context.Culture);
                    }

                    product.CustomFields.Add(field);
                }
            }

            return product;
        }

        protected override void MapProperty(System.Reflection.PropertyInfo targetProperty, object source, object target, Type sourceType, Type targetType, string propertyPath, MappingContext context)
        {
            if (targetProperty.Name == "Categories" && context.Includes.Includes(propertyPath))
            {
                var fromProduct = source as Kooboo.Commerce.Products.Product;
                var product = target as Product;

                foreach (var fromCategory in fromProduct.Categories)
                {
                    var mapper = GetMapperOrDefault(typeof(Kooboo.Commerce.Categories.Category), typeof(Category));
                    var category = mapper.Map(fromCategory, new Category(), typeof(Kooboo.Commerce.Categories.Category), typeof(Category), null, new MappingContext()) as Category;
                    product.Categories.Add(category);
                }
            }
            else
            {
                base.MapProperty(targetProperty, source, target, sourceType, targetType, propertyPath, context);
            }
        }
    }
}
