using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Local.Mapping;
using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Carts.Mapping
{
    public class ShoppingCartItemMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            var model = base.Map(source, target, sourceType, targetType, prefix, context) as ShoppingCartItem;
            var cartItem = source as Kooboo.Commerce.Carts.ShoppingCartItem;

            // Product
            var mapper = GetMapperOrDefault(typeof(Kooboo.Commerce.Products.Product), typeof(Product));
            var propPath = prefix + "Product";
            var includes = new IncludeCollection();
            foreach (var each in context.Includes)
            {
                if (each.StartsWith(propPath) && each.Length > propPath.Length && each[propPath.Length] == '.')
                {
                    includes.Add(each.Substring(propPath.Length + 1));
                }
            }

            model.Product = mapper.Map(cartItem.ProductVariant.Product, new Product(), null, new MappingContext(context.ApiContext, includes)) as Product;

            return model;
        }
    }
}
