using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public abstract class CommerceAPIBase : ICommerceAPI
    {
        public abstract void InitCommerceInstance(string instance, string language);

        protected abstract T GetAPI<T>() where T : class;

        public ICountryAPI Country
        {
            get { return GetAPI<ICountryAPI>(); }
        }

        public IBrandAPI Brand
        {
            get { return GetAPI<IBrandAPI>(); }
        }

        public ICategoryAPI Category
        {
            get { return GetAPI<ICategoryAPI>(); }
        }

        public IPaymentMethodAPI PaymentMethod
        {
            get { return GetAPI<IPaymentMethodAPI>(); }
        }

        public ICustomerAPI Customer
        {
            get { return GetAPI<ICustomerAPI>(); }
        }

        public IProductAPI Product
        {
            get { return GetAPI<IProductAPI>(); }
        }

        public ICartAPI Cart
        {
            get { return GetAPI<ICartAPI>(); }
        }

        public IOrderAPI Order
        {
            get { return GetAPI<IOrderAPI>(); }
        }

        public IPaymentAPI Payment
        {
            get { return GetAPI<IPaymentAPI>(); }
        }
    }
}
