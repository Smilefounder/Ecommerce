﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentMethodQuery : ICommerceQuery<PaymentMethod>
    {
        /// <summary>
        /// Filter the payment methods by id.
        /// </summary>
        IPaymentMethodQuery ById(int id);
    }
}
