using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.API.CmsSite
{
    public class CmsSiteUrlResolver
    {
        public static string MapUrl(IItemResource resource, string linkRelName, string url = null)
        {
            if (resource.Links == null)
                return url;
            var link = resource.Links.FirstOrDefault(o => o.Rel.ToLower() == linkRelName.ToLower());
            if (link != null)
            {
                var type = resource.GetType();
                if (type == typeof(Categories.Category))
                    return MapCategoryUrl(resource, link);
                else if (type == typeof(Brands.Brand))
                    return MapBrandUrl(resource, link);
                else if (type == typeof(Products.Product))
                    return MapProductUrl(resource, link);
                else if (type == typeof(ShoppingCarts.ShoppingCart))
                    return MapShoppingCartUrl(resource, link);
                else if (type == typeof(Orders.Order))
                    return MapOrderUrl(resource, link);
            }
            return url;
        }

        private static string MapCategoryUrl(IItemResource resource, Link link)
        {
            return link.Href;
        }

        private static string MapBrandUrl(IItemResource resource, Link link)
        {
            return link.Href;
        }

        private static string MapProductUrl(IItemResource resource, Link link)
        {
            return link.Href;
        }

        private static string MapShoppingCartUrl(IItemResource resource, Link link)
        {
            return link.Href;
        }

        private static string MapOrderUrl(IItemResource resource, Link link)
        {
            return link.Href;
        }
    }
}
