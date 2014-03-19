using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments.Services
{
    public interface IPaymentMethodService
    {
        PaymentMethod GetById(int id);

        IEnumerable<PaymentMethod> GetAllPaymentMethods();

        IQueryable<PaymentMethod> Query();

        void Enable(PaymentMethod method);

        void Disable(PaymentMethod method);

        bool Create(PaymentMethod method);

        bool Update(PaymentMethod method);

        bool Delete(PaymentMethod method);
    }
}
