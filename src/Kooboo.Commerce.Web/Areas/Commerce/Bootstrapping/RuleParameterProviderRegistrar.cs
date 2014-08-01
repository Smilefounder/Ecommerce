using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class RuleParameterProviderRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            foreach (var type in typeFinder.FindClassesOfType<IRuleParameterProvider>())
            {
                if (type != typeof(DefaultRuleParameterProvider))
                {
                    var provider = Activator.CreateInstance(type) as IRuleParameterProvider;
                    RuleParameterProviders.Providers.Add(provider);
                }
            }
        }
    }
}