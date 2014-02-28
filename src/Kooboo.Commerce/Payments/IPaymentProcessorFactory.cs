using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessorFactory
    {
        IEnumerable<IPaymentProcessor> All();

        IPaymentProcessor FindByName(string name);
    }

    [Dependency(typeof(IPaymentProcessorFactory))]
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        private IEngine _engine;

        public PaymentProcessorFactory()
            : this(EngineContext.Current)
        {
        }

        public PaymentProcessorFactory(IEngine engin)
        {
            _engine = engin;
        }

        public IEnumerable<IPaymentProcessor> All()
        {
            return _engine.ResolveAll<IPaymentProcessor>();
        }

        public IPaymentProcessor FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name == name);
        }
    }
}
