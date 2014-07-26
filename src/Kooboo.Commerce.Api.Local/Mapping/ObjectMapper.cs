using Kooboo.Commerce.Api.Local.Products.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public static class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source, IncludeCollection includes, CultureInfo culture)
        {
            return Map<TSource, TTarget>(source, (TTarget)Activator.CreateInstance(typeof(TTarget)), includes, culture);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target, IncludeCollection includes, CultureInfo culture)
        {
            return (TTarget)Map(source, target, typeof(TSource), typeof(TTarget), includes, culture);
        }

        public static object Map(object source, Type sourceType, Type targetType, IncludeCollection includes, CultureInfo culture)
        {
            return Map(source, Activator.CreateInstance(targetType), sourceType, targetType, includes, culture);
        }

        public static object Map(object source, object target, Type sourceType, Type targetType, IncludeCollection includes, CultureInfo culture)
        {
            var mapper = GetMapper(sourceType, targetType);
            return mapper.Map(source, target, sourceType, targetType, null, new MappingContext(includes, culture));
        }

        // Key: Tuple<SourceType, TargetType>
        static readonly Dictionary<Tuple<Type, Type>, IObjectMapper> _mappers = new Dictionary<Tuple<Type, Type>, IObjectMapper>();

        public static IObjectMapper Default = new DefaultObjectMapper();

        public static IObjectMapper GetMapper(Type sourceType, Type targetType)
        {
            IObjectMapper mapper;
            if (_mappers.TryGetValue(Tuple.Create<Type, Type>(sourceType, targetType), out mapper))
            {
                return mapper;
            }

            return Default;
        }

        public static void AddMapper(Type sourceType, Type targetType, IObjectMapper mapper)
        {
            _mappers.Add(Tuple.Create<Type, Type>(sourceType, targetType), mapper);
        }

        static ObjectMapper()
        {
            AddMapper(typeof(Kooboo.Commerce.Products.Product), typeof(Kooboo.Commerce.Api.Products.Product), new ProductMapper());
            AddMapper(typeof(Kooboo.Commerce.Products.ProductPrice), typeof(Kooboo.Commerce.Api.Products.ProductPrice), new ProductVariantMapper());
        }
    }
}
