using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Context
{
    public interface ICurrentInstanceProvider
    {
        CommerceInstance GetCurrentInstance();
    }
}
