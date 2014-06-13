using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Initialization
{
    public interface ICommerceInstanceInitializer
    {
        void Initialize(CommerceInstance instance);
    }
}
