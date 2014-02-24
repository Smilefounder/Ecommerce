using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IShippingRateProviderViewsFactory
    {
        IShippingRateProviderViews FindByProviderName(string providerName);
    }

    [Dependency(typeof(IShippingRateProviderViewsFactory))]
    public class ShippingRateProviderViewsFactory : IShippingRateProviderViewsFactory
    {
        private IEngine _engine;
        private List<IShippingRateProviderViews> _views;

        public ShippingRateProviderViewsFactory()
            : this(EngineContext.Current) { }

        public ShippingRateProviderViewsFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IShippingRateProviderViews FindByProviderName(string providerName)
        {
            if (_views == null)
            {
                _views = _engine.ResolveAll<IShippingRateProviderViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.ProviderName.Equals(providerName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
