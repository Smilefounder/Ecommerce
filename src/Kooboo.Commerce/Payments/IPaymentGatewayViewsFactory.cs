using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentGatewayViewsFactory
    {
        IPaymentGatewayViews FindByPaymentGateway(string paymentGatewayName);
    }

    [Dependency(typeof(IPaymentGatewayViewsFactory))]
    public class PaymentGatewayViewsFactory : IPaymentGatewayViewsFactory
    {
        private IEngine _engine;
        private List<IPaymentGatewayViews> _views;

        public PaymentGatewayViewsFactory()
            : this(EngineContext.Current) { }

        public PaymentGatewayViewsFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
        }

        public IPaymentGatewayViews FindByPaymentGateway(string paymentGatewayName)
        {
            Require.NotNullOrEmpty(paymentGatewayName, "paymentGatewayName");

            if (_views == null)
            {
                _views = _engine.ResolveAll<IPaymentGatewayViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.PaymentGatewayName.Equals(paymentGatewayName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
