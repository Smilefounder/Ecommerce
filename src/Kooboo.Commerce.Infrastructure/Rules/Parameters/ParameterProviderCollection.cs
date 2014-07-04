using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class ParameterProviderCollection : Collection<IParameterProvider>
    {
        public ParameterProviderCollection()
        {
        }

        public ParameterProviderCollection(IEnumerable<IParameterProvider> providers)
            : base(providers.ToList())
        {
        }

        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
        {
            foreach (var provider in this)
            {
                foreach (var param in provider.GetParameters(dataContextType))
                {
                    yield return param;
                }
            }
        }

        public ConditionParameter GetParameter(Type dataContextType, string paramName)
        {
            return GetParameters(dataContextType).FirstOrDefault(p => p.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
