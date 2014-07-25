using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public static class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source, IncludeCollection includes)
        {
            return Map<TSource, TTarget>(source, (TTarget)Activator.CreateInstance(typeof(TTarget)), includes);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target, IncludeCollection includes)
        {
            return (TTarget)Map(source, target, typeof(TSource), typeof(TTarget), includes);
        }

        public static object Map(object source, Type sourceType, Type targetType, IncludeCollection includes)
        {
            return Map(source, Activator.CreateInstance(targetType), sourceType, targetType, includes);
        }

        public static object Map(object source, object target, Type sourceType, Type targetType, IncludeCollection includes)
        {
            var mapper = GetMapper(sourceType, targetType);
            return mapper.Map(source, target, sourceType, targetType, null, new MappingContext(includes));
        }

        static readonly Dictionary<Tuple<Type, Type>, IObjectMapper> _mappers = new Dictionary<Tuple<Type, Type>, IObjectMapper>();

        public static IObjectMapper Default = new LocalizableObjectMapper();

        public static IObjectMapper GetMapper(Type sourceType, Type targetType)
        {
            IObjectMapper mapper;
            if (_mappers.TryGetValue(Tuple.Create<Type, Type>(sourceType, targetType), out mapper))
            {
                return mapper;
            }

            return Default;
        }
    }
}
