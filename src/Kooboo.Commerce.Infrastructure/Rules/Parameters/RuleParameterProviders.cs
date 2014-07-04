using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class RuleParameterProviders
    {
        static readonly RuleParameterProviderCollection _providers = new RuleParameterProviderCollection
        {
            new DefaultRuleParameterProvider()
        };

        public static RuleParameterProviderCollection Providers
        {
            get
            {
                return _providers;
            }
        }
    }
}
