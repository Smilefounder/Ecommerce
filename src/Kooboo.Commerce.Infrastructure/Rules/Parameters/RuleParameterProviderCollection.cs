using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class RuleParameterProviderCollection : Collection<IRuleParameterProvider>
    {
        public RuleParameterProviderCollection()
        {
        }

        public RuleParameterProviderCollection(IEnumerable<IRuleParameterProvider> providers)
            : base(providers.ToList())
        {
        }

        public IEnumerable<RuleParameter> GetParameters(Type dataContextType)
        {
            foreach (var provider in this)
            {
                foreach (var param in provider.GetParameters(dataContextType))
                {
                    yield return param;
                }
            }
        }

        public RuleParameter GetParameter(Type dataContextType, string paramName)
        {
            return GetParameters(dataContextType).FirstOrDefault(p => p.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
