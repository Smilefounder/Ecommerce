using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public static class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            return Map<TSource, TTarget>(source, (TTarget)Activator.CreateInstance(typeof(TTarget)));
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return (TTarget)Map(source, target, typeof(TSource), typeof(TTarget));
        }

        public static object Map(object source, Type sourceType, Type targetType)
        {
            return Map(source, Activator.CreateInstance(targetType), sourceType, targetType);
        }

        public static object Map(object source, object target, Type sourceType, Type targetType)
        {
            var mapper = GetMapper(sourceType, targetType);
            return mapper.Map(source, target, sourceType, targetType, new MappingContext());
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
