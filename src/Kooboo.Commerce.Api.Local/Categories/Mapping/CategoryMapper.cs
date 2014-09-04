using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Api.Local.Categories.Mapping
{
    public class CategoryMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            // Parent ID
            var model = base.Map(source, target, sourceType, targetType, prefix, context) as Category;
            var category = source as Core.CategoryTreeNode;
            if (category.Parent != null)
            {
                model.ParentId = category.Parent.Id;
            }

            // Localization
            var texts = Localizer.GetText(new EntityKey(typeof(Core.Category), model.Id), new[] { "Name", "Description" }, context.ApiContext.Culture);
            model.Name = texts["Name"] ?? model.Name;
            model.Description = texts["Description"] ?? model.Description;

            return model;
        }

        protected override bool IsComplexPropertyIncluded(System.Reflection.PropertyInfo property, string propertyPath, MappingContext context)
        {
            if (property.Name == "Children" && context.Includes.Includes("Subtrees"))
            {
                return true;
            }

            return base.IsComplexPropertyIncluded(property, propertyPath, context);
        }
    }
}
