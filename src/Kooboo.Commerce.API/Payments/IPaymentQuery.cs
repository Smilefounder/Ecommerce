using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentQuery : ICommerceQuery<Payment>
    {
        IPaymentQuery By(PaymentQueryParams parameters);

        IPaymentQuery ById(int id);

        IPaymentQuery ByTarget(string targetType, string targetId);

        IPaymentQuery ByStatus(PaymentStatus status);
    }
}
