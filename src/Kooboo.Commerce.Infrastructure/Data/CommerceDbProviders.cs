using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public static class CommerceDbProviders
    {
        static readonly CommerceDbProviderCollection _providers = new CommerceDbProviderCollection();

        public static CommerceDbProviderCollection Providers
        {
            get
            {
                return _providers;
            }
        }
    }
}
