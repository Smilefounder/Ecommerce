using Kooboo.Commerce.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public interface IObjectMapper
    {
        object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context);
    }

    public static class ObjectMapperExtensions
    {
        public static object Map(this IObjectMapper mapper, object source, object target, string prefix, MappingContext context)
        {
            return mapper.Map(source, target, TypeHelper.GetType(source), target.GetType(), prefix, context);
        }
    }
}
