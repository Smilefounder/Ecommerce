using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceContext : IDisposable
    {
        CommerceInstance CurrentInstance { get; }
    }
}
