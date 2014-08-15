using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Categories.Mapping
{
    public class CategoryMapper : DefaultObjectMapper
    {
        protected override bool IsComplexPropertyIncluded(System.Reflection.PropertyInfo property, string propertyPath, MappingContext context)
        {
            if (property.Name == "Children" && context.Includes.Includes("WholeCategoryTree"))
            {
                return true;
            }

            return base.IsComplexPropertyIncluded(property, propertyPath, context);
        }
    }
}
