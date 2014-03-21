using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.ShoppingCarts
{
    [Dependency(typeof(IShoppingCartAPI), ComponentLifeStyle.Transient)]
    public class ShoppingCartAPI : LocalCommerceQuery<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart>, IShoppingCartAPI
    {
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;
        private IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> _mapper;
        private IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> _cartItemMapper;
        private IMapper<Customer, Kooboo.Commerce.Customers.Customer> _customerMapper;
        private bool _loadWithCutomer = false;

        public ShoppingCartAPI(IShoppingCartService shoppingCartService, ICustomerService customerService,
            IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> mapper,
            IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> cartItemMapper,
            IMapper<Customer, Kooboo.Commerce.Customers.Customer> customerMapper)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _mapper = mapper;
            _cartItemMapper = cartItemMapper;
            _customerMapper = customerMapper;
        }

        protected override IQueryable<Commerce.ShoppingCarts.ShoppingCart> CreateQuery()
        {
            return _shoppingCartService.Query();
        }

        protected override IQueryable<Commerce.ShoppingCarts.ShoppingCart> OrderByDefault(IQueryable<Commerce.ShoppingCarts.ShoppingCart> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        protected override ShoppingCart Map(Commerce.ShoppingCarts.ShoppingCart obj)
        {
            List<string> includeComplexPropertyNames = new List<string>();
            includeComplexPropertyNames.Add("Items");
            includeComplexPropertyNames.Add("Items.ProductPrice");
            includeComplexPropertyNames.Add("Items.ProductPrice.Product");
            includeComplexPropertyNames.Add("ShippingAddress");
            includeComplexPropertyNames.Add("BillingAddress");
            if (_loadWithCutomer)
            {
                includeComplexPropertyNames.Add("Customer");
            }

            return _mapper.MapTo(obj, includeComplexPropertyNames.ToArray());
        }

        protected override void OnQueryExecuted()
        {
            _loadWithCutomer = false;
        }

        public IShoppingCartQuery LoadWithCustomer()
        {
            _loadWithCutomer = true;
            return this;
        }

        public IShoppingCartQuery BySessionId(string sessionId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.SessionId == sessionId);
            return this;
        }

        public IShoppingCartQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        public bool AddCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                return _shoppingCartService.AddCartItem(cartId, _cartItemMapper.MapFrom(item));
            }
            return false;
        }

        public bool UpdateCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                return _shoppingCartService.UpdateCartItem(cartId, _cartItemMapper.MapFrom(item));
            }
            return false;
        }

        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            return _shoppingCartService.RemoveCartItem(cartId, cartItemId);
        }

        //public override bool Create(ShoppingCart obj)
        //{
        //    if (obj != null)
        //    {
        //        return _shoppingCartService.Create(_mapper.MapFrom(obj));
        //    }
        //    return false;
        //}

        //public override bool Update(ShoppingCart obj)
        //{
        //    if (obj != null)
        //    {
        //        return _shoppingCartService.Update(_mapper.MapFrom(obj));
        //    }
        //    return false;
        //}

        //public override bool Save(ShoppingCart obj)
        //{
        //    if (obj != null)
        //    {
        //        return _shoppingCartService.Save(_mapper.MapFrom(obj));
        //    }
        //    return false;
        //}

        //public override bool Delete(ShoppingCart obj)
        //{
        //    if (obj != null)
        //    {
        //        return _shoppingCartService.Delete(_mapper.MapFrom(obj));
        //    }
        //    return false;
        //}


        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.Query().Where(o => o.AccountId == accountId).FirstOrDefault();

                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.AddToCart(sessionId, customerId, productPriceId, quantity);
        }

        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.Query().Where(o => o.AccountId == accountId).FirstOrDefault();

                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.UpdateCart(sessionId, customerId, productPriceId, quantity);
        }

        public bool FillCustomerByAccount(string sessionId, Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            return _shoppingCartService.FillCustomerByAccount(sessionId, user);
        }


        public bool ExpireShppingCart(int shoppingCartId)
        {
            var shoppingCart = _shoppingCartService.Query().Where(o => o.Id == shoppingCartId).FirstOrDefault();
            if(shoppingCart != null)
            {
                return _shoppingCartService.ExpireShppingCart(shoppingCart);
            }
            return false;
        }

        public IShoppingCartQuery Query()
        {
            return this;
        }

        public IShoppingCartAccess Access()
        {
            return this;
        }
    }
}
