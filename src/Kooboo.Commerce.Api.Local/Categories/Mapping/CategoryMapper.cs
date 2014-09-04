using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Categories.Mapping
{
    public class CategoryMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            // Parent ID
            var model = base.Map(source, target, sourceType, targetType, prefix, context) as Category;
            var category = source as Kooboo.Commerce.Categories.CategoryTreeNode;
            if (category.Parent != null)
            {
                model.ParentId = category.Parent.Id;
            }

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
