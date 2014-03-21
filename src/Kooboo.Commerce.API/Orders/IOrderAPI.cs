using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    public interface IOrderAPI : IOrderQuery, IOrderAccess
    {
        IOrderQuery Query();
        IOrderAccess Access();
    }
}
