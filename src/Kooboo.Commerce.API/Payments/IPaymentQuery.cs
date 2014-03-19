using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    // TODO: OrderBy
    public interface IPaymentQuery : ICommerceQuery<Payment>
    {
        IPaymentQuery ById(int id);

        IPaymentQuery ByTarget(string targetType, string targetId);
    }
}
