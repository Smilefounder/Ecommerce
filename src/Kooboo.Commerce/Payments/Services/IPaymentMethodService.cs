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

        void Create(PaymentMethod method);

        void Delete(PaymentMethod method);

        bool Enable(PaymentMethod method);

        bool Disable(PaymentMethod method);
    }
}
