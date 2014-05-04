using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments.Services
{
    public interface IPaymentMethodService
    {
        PaymentMethod GetById(int id);

        IQueryable<PaymentMethod> Query();

        bool Create(PaymentMethod method);

        bool Delete(PaymentMethod method);
    }
}
