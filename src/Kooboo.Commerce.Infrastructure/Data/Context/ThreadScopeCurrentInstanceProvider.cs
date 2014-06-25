using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Context
{
    public class ThreadScopeCurrentInstanceProvider : ICurrentInstanceProvider
    {
        public CommerceInstance GetCurrentInstance()
        {
            return Scope.Current<CommerceInstance>();
        }
    }
}
