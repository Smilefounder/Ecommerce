using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class QueryDescriptors
    {
        static readonly Dictionary<Type, IQueryDescriptor> _descriptorsByQueryType = new Dictionary<Type, IQueryDescriptor>();

        public static IQueryDescriptor Get(Type queryType)
        {
            IQueryDescriptor descriptor = null;
            if (_descriptorsByQueryType.TryGetValue(queryType, out descriptor))
            {
                return descriptor;
            }

            return null;
        }

        public static void Add(Type queryType, IQueryDescriptor descriptor)
        {
            _descriptorsByQueryType.Add(queryType, descriptor);
        }

        static QueryDescriptors()
        {
            Add(typeof(Query<Brand>), new BrandQueryDescriptor());
            Add(typeof(ShoppingCart), new ShoppingCartQueryDescriptor());
        }
    }
}
