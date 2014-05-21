using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IShippingRateProviderFactory
    {
        IEnumerable<IShippingRateProvider> All();

        IShippingRateProvider FindByName(string name);
    }

    [Dependency(typeof(IShippingRateProviderFactory))]
    public class DefaultShippingRateProviderFactory : IShippingRateProviderFactory
    {
        public IEnumerable<IShippingRateProvider> All()
        {
            return EngineContext.Current.ResolveAll<IShippingRateProvider>();
        }

        public IShippingRateProvider FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
