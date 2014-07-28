using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Payments
{
    public interface IPaymentQuery : ICommerceQuery<Payment>
    {
        IPaymentQuery ById(int id);

        IPaymentQuery ByStatus(PaymentStatus status);
    }
}
