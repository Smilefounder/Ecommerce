using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Countries;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Metadata
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
            Add(typeof(Query<Category>), new CategoryQueryDescriptor());
            Add(typeof(Query<Country>), new CountryQueryDescriptor());
            Add(typeof(Query<Customer>), new CustomerQueryDescriptor());
            Add(typeof(Query<ShoppingCart>), new ShoppingCartQueryDescriptor());
            Add(typeof(Query<Order>), new OrderQueryDescriptor());
            Add(typeof(Query<PaymentMethod>), new PaymentMethodQueryDescriptor());
            Add(typeof(Query<ShippingMethod>), new ShippingMethodQueryDescriptor());
            Add(typeof(Query<Product>), new ProductQueryDescriptor());
        }
    }
}
