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
    }
}
