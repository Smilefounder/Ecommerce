﻿using Kooboo.Commerce.API.Brands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface ICommerceAPI
    {
        void InitCommerceInstance(string instance, string language);

        //ICountryAPI Country { get; }
        IBrandQuery Brands { get; }
        //ICategoryAPI Category { get; }
        //IPaymentMethodAPI PaymentMethod { get; }
        //ICustomerAPI Customer { get; }
        //IProductAPI Product { get; }
        //ICartAPI Cart { get; }
        //IOrderAPI Order { get; }
        //IPaymentAPI Payment { get; }
    }
}
