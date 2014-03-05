using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Kooboo.Commerce.API
{
    public interface ICommerceService
    {
        IEnumerable<Category> GetAllCategories(string instance, string language, int level = 1);

        IEnumerable<Category> GetSubCategories(string instance, string language, int parentCategoryId);

        Category GetCategory(string instance, string language, int categoryId, bool loadParents = false);

        IEnumerable<Brand> GetAllBrands(string instance, string language);

        IPagedList<Product> SearchProducts(string instance, string language, string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50);

        Product GetProductById(string instance, string language, int id);

        Customer GetCustomerByAccountId(string instance, string language, int accountId);

        Customer GetCustomerById(string instance, string language, int customerId);

        bool AddToCart(string instance, string language, Guid? guestId, int? customerId, int productPriceId, int quantity);

        bool UpdateCart(string instance, string language, Guid? guestId, int? customerId, int productPriceId, int quantity);

        bool FillCustomerByAccount(string instance, string language, Guid guestId, int accountId);

        ShoppingCart GetMyCart(string instance, string language, Guid? guestId, int? customerId);

        Order CreateOrderFromShoppingCart(string instance, string language, int shoppingCartId);
    }
}
