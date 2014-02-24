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
    public class ShippingRateProviderFactory : IShippingRateProviderFactory
    {
        private IEngine _engine;

        public ShippingRateProviderFactory()
            : this(EngineContext.Current) { }

        public ShippingRateProviderFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IShippingRateProvider> All()
        {
            return _engine.ResolveAll<IShippingRateProvider>();
        }

        public IShippingRateProvider FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name == name);
        }
    }
}
