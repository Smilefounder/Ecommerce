using System.Web;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api.Local.Countries;
using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Local.Categories;
using Kooboo.Commerce.Api.Local.Customers;
using Kooboo.Commerce.Api.Local.Products;
using Kooboo.Commerce.Api.Local.Carts;
using Kooboo.Commerce.Api.Local.Orders;
using Kooboo.Commerce.Api.Local.Payments;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Api.Local.Shipping;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Api.Shipping;
using Kooboo.Commerce.Api.Countries;
using Kooboo.Commerce.Api.Local.Brands;

namespace Kooboo.Commerce.Api.Local
{
    /// <summary>
    /// Local commerce api which directly dependent on commerce native componements.
    /// </summary>
    [Dependency(typeof(ICommerceApi), Key = "Local")]
    public class LocalCommerceApi : ICommerceApi
    {
        private LocalApiContext _context;

        public void Initialize(Api.ApiContext context)
        {
            // TODO: Not good! But without this, we are not able to resolve services.
            //       So we might need to think about improving the design of service apis. No idea for now.
            HttpContext.Current.Items["instance"] = context.InstanceName;
            HttpContext.Current.Items["language"] = context.Culture.Name;

            _context = new LocalApiContext(context, CommerceInstance.Current);
        }

        public ICountryApi Countries
        {
            get
            {
                return new CountryApi(_context);
            }
        }

        public IBrandApi Brands
        {
            get
            {
                return new BrandApi(_context);
            }
        }

        public ICategoryApi Categories
        {
            get
            {
                return new CategoryApi(_context);
            }
        }

        public ICustomerApi Customers
        {
            get
            {
                return new CustomerApi(_context);
            }
        }

        public IProductApi Products
        {
            get
            {
                return new ProductApi(_context);
            }
        }

        public IShoppingCartApi ShoppingCarts
        {
            get
            {
                return new ShoppingCartApi(_context, Customers);
            }
        }

        public IOrderApi Orders
        {
            get
            {
                return new OrderApi(_context, EngineContext.Current.Resolve<IPaymentProcessorProvider>());
            }
        }

        public IPaymentMethodApi PaymentMethods
        {
            get
            {
                return new PaymentMethodApi(_context);
            }
        }

        public IShippingMethodApi ShippingMethods
        {
            get
            {
                return new ShippingMethodApi(_context);
            }
        }
    }
}
