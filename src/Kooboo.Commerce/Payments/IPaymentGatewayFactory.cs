using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentGatewayFactory
    {
        IEnumerable<IPaymentGateway> All();

        IPaymentGateway FindByName(string name);
    }

    [Dependency(typeof(IPaymentGatewayFactory))]
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private IEngine _engine;

        public PaymentGatewayFactory()
            : this(EngineContext.Current)
        {
        }

        public PaymentGatewayFactory(IEngine engin)
        {
            _engine = engin;
        }

        public IEnumerable<IPaymentGateway> All()
        {
            return _engine.ResolveAll<IPaymentGateway>();
        }

        public IPaymentGateway FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name == name);
        }
    }
}
