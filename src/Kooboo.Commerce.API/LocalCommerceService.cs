using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo;
using Kooboo.Commerce.Accounts;
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
using Kooboo.Commerce.Accounts.Services;
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
            if(HttpContext.Current != null)
            {
                HttpContext.Current.Items[HttpCommerceInstanceNameResolverBase.DefaultParamName] = instance;
                HttpContext.Current.Items["language"] = language;
            }
        }

        private T GetService<T>(params Parameter[] paras) where T : class
        {
            return EngineContext.Current.Resolve<T>(paras);
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
                while(p.Parent != null)
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


        public Customer GetCustomerByAccountId(string instance, string language, int accountId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICustomerService>();
            return svr.GetByAccountId(accountId);
        }

        public Customer GetCustomerById(string instance, string language, int customerId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<ICustomerService>();
            return svr.GetById(customerId);
        }


        public bool AddToCart(string instance, string language, Guid? guestId, int? customerId, int productPriceId, int quantity)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            return svr.AddToCart(guestId, customerId, productPriceId, quantity);
        }

        public bool UpdateCart(string instance, string language, Guid? guestId, int? customerId, int productPriceId, int quantity)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            return svr.UpdateCart(guestId, customerId, productPriceId, quantity);
        }

        public bool FillCustomerByAccount(string instance, string language, Guid guestId, int accountId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            return svr.FillCustomerByAccount(guestId, accountId);
        }

        public ShoppingCart GetMyCart(string instance, string language, Guid? guestId, int? customerId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IShoppingCartService>();
            if (customerId.HasValue)
                return svr.GetByCustomer(customerId.Value);
            else if (guestId.HasValue)
                return svr.GetByGuestId(guestId.Value);
            return null;
        }

        public Order CreateOrderFromShoppingCart(string instance, string language, int shoppingCartId)
        {
            InitCommerceInstance(instance, language);
            var svr = GetService<IOrderService>();
            return svr.CreateOrderFromShoppingCart(shoppingCartId);
        }
    }
}
