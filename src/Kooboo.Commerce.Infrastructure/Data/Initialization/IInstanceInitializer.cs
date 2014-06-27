using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Initialization
{
    public interface IInstanceInitializer
    {
        void Initialize(CommerceInstance instance);
    }
}
