using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    public interface ICustomerAPI : ICustomerQuery, ICustomerAccess
    {
        ICustomerQuery Query();
        ICustomerAccess Access();
    }
}
