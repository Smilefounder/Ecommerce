using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.CMSIntegration
{
    public interface IShoppingCartSessionIdProvider
    {
        string GetCurrentSessionId(bool ensure);
    }
}
