using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Local.Carts.Mapping;
using Kooboo.Commerce.Api.Local.Products.Mapping;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public static class ObjectMapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source, ApiContext apiContext, IncludeCollection includes)
        {
            return Map<TSource, TTarget>(source, (TTarget)Activator.CreateInstance(typeof(TTarget)), apiContext, includes);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target, ApiContext apiContext, IncludeCollection includes)
        {
            return (TTarget)Map(source, target, typeof(TSource), typeof(TTarget), apiContext, includes);
        }

        public static object Map(object source, Type sourceType, Type targetType, ApiContext apiContext, IncludeCollection includes)
        {
            return Map(source, Activator.CreateInstance(targetType), sourceType, targetType, apiContext, includes);
        }

        public static object Map(object source, object target, Type sourceType, Type targetType, ApiContext apiContext, IncludeCollection includes)
        {
            var mapper = GetMapper(sourceType, targetType);
            return mapper.Map(source, target, sourceType, targetType, null, new MappingContext(apiContext, includes));
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
            AddMapper(typeof(Kooboo.Commerce.Products.Product), typeof(Product), new ProductMapper());
            AddMapper(typeof(Kooboo.Commerce.Products.ProductVariant), typeof(ProductVariant), new ProductVariantMapper());
            AddMapper(typeof(Kooboo.Commerce.Carts.ShoppingCart), typeof(ShoppingCart), new ShoppingCartMapper());
            AddMapper(typeof(Kooboo.Commerce.Carts.ShoppingCartItem), typeof(ShoppingCartItem), new ShoppingCartItemMapper());
        }
    }
}
