using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Pricing;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API
{
    [Dependency(typeof(ICommerceService))]
    public class LocalCommerceService : ICommerceService
    {
        public LocalCommerceService()
        {
        }

        private void InitCommerceInstance(string instance, string language)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[HttpCommerceInstanceNameResolverBase.DefaultParamName] = instance;
                HttpContext.Current.Items["language"] = language;
            }
        }

        private T GetService<T>(params Parameter[] paras) where T : class
        {
            return EngineContext.Current.Resolve<T>(paras);
        }

        public IEnumerable<Country> GetAllCountries(string instance, string language)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICountryService>();
            return svr.GetAllCountries();
        }

        public IEnumerable<Category> GetAllCategories(string instance, string language, int level = 1)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICategoryService>();
            var categories = svr.GetRootCategories();
            return categories;
        }

        public IEnumerable<Category> GetSubCategories(string instance, string language, int parentCategoryId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICategoryService>();
            var categories = svr.GetChildCategories(parentCategoryId);
            return categories;
        }

        public Category GetCategory(string instance, string language, int categoryId, bool loadParents = false)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICategoryService>();
            var category = svr.GetById(categoryId);
            if (category != null && loadParents)
            {
                var p = category;
                while (p.Parent != null)
                {
                    p = p.Parent;
                }

            }
            return category;
        }

        public IEnumerable<Brand> GetAllBrands(string instance, string language)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IBrandService>();
            return svr.GetAllBrands();
        }

        public IPagedList<Product> SearchProducts(string instance, string language, string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IProductService>();
            return svr.GetAllProducts(userInput, categoryId, pageIndex, pageSize);
        }

        public Product GetProductById(string instance, string language, int id)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IProductService>();
            return svr.GetById(id);
        }

        public Customer GetCustomerById(string instance, string language, int customerId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICustomerService>();
            return svr.GetById(customerId, true);
        }

        public Customer GetCustomerByAccount(string instance, string language, MembershipUser user)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICustomerService>();
            return svr.GetByAccountId(user.UUID, true);
        }

        public bool AddToCart(string instance, string language, string sessionId, MembershipUser user, int productPriceId, int quantity)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            int? customerId = null;
            if (user != null)
            {
                var customerSvr = GetService<ICustomerService>();
                var customer = customerSvr.GetByAccountId(user.UUID, false);
                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return svr.AddToCart(sessionId, customerId, productPriceId, quantity);
        }

        public bool UpdateCart(string instance, string language, string sessionId, MembershipUser user, int productPriceId, int quantity)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            int? customerId = null;
            if (user != null)
            {
                var customerSvr = GetService<ICustomerService>();
                var customer = customerSvr.GetByAccountId(user.UUID, false);
                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return svr.UpdateCart(sessionId, customerId, productPriceId, quantity);
        }

        public ShoppingCart GetMyCart(string instance, string language, string sessionId, MembershipUser user)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            if (user != null)
            {
                var customerSvr = GetService<ICustomerService>();
                var customer = customerSvr.GetByAccountId(user.UUID, false);
                if (customer != null)
                {
                    return svr.GetByCustomer(customer.Id);
                }
            }
            if (!string.IsNullOrEmpty(sessionId))
                return svr.GetBySessionId(sessionId);
            return null;
        }

        public Order GetMyOrder(string instance, string language, string sessionId, MembershipUser user)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IOrderService>();
            var shoppingCart = GetMyCart(instance, language, sessionId, user);
            if (shoppingCart != null)
            {
                var order = svr.GetByShoppingCartId(shoppingCart.Id);
                if (order == null)
                    order = svr.CreateOrderFromShoppingCart(shoppingCart, user);
                return order;
            }
            return null;
        }

        public bool SaveOrder(string instance, string language, Order order)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IOrderService>();
            try
            {
                svr.Save(order);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<PaymentMethod> GetAllPaymentMethods(string instance, string language)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IPaymentMethodService>();
            return svr.GetAllPaymentMethods();
        }
    }
}
