using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class ParameterProviders
    {
        static readonly ParameterProviderCollection _providers = new ParameterProviderCollection
        {
            new DefaultParameterProvider()
        };

        public static ParameterProviderCollection Providers
        {
            get
            {
                return _providers;
            }
        }
    }
}
