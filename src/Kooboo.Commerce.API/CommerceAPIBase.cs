using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Brands.Services;

namespace Kooboo.Commerce.API
{
    public abstract class CommerceAPIBase : ICommerceAPI
    {
        public abstract void InitCommerceInstance(string instance, string language);

        protected abstract TQuery GetAPI<TQuery, TModel>() where TQuery : ICommerceQuery<TModel>;

        //public ICountryAPI Country
        //{
        //    get { return GetAPI<ICountryAPI>(); }
        //}

        public IBrandQuery Brand
        {
            get { return GetAPI<IBrandQuery, Brand>(); }
        }

        //public ICategoryAPI Category
        //{
        //    get { return GetAPI<ICategoryAPI>(); }
        //}

        //public IPaymentMethodAPI PaymentMethod
        //{
        //    get { return GetAPI<IPaymentMethodAPI>(); }
        //}

        //public ICustomerAPI Customer
        //{
        //    get { return GetAPI<ICustomerAPI>(); }
        //}

        //public IProductAPI Product
        //{
        //    get { return GetAPI<IProductAPI>(); }
        //}

        //public ICartAPI Cart
        //{
        //    get { return GetAPI<ICartAPI>(); }
        //}

        //public IOrderAPI Order
        //{
        //    get { return GetAPI<IOrderAPI>(); }
        //}

        //public IPaymentAPI Payment
        //{
        //    get { return GetAPI<IPaymentAPI>(); }
        //}
    }
}
