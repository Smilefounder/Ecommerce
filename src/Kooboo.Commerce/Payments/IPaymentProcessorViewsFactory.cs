using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessorViewsFactory
    {
        IPaymentProcessorViews FindByPaymentProcessor(string processorName);
    }

    [Dependency(typeof(IPaymentProcessorViewsFactory))]
    public class PaymentProcessorViewsFactory : IPaymentProcessorViewsFactory
    {
        private IEngine _engine;
        private List<IPaymentProcessorViews> _views;

        public PaymentProcessorViewsFactory()
            : this(EngineContext.Current) { }

        public PaymentProcessorViewsFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
        }

        public IPaymentProcessorViews FindByPaymentProcessor(string processorName)
        {
            Require.NotNullOrEmpty(processorName, "processorName");

            if (_views == null)
            {
                _views = _engine.ResolveAll<IPaymentProcessorViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.PaymentProcessorName.Equals(processorName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
