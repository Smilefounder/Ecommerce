using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessorProvider
    {
        IEnumerable<IPaymentProcessor> All();

        IPaymentProcessor FindByName(string processorName);
    }

    [Dependency(typeof(IPaymentProcessorProvider))]
    public class DefaultPaymentProcessorProvider : IPaymentProcessorProvider
    {
        public IEnumerable<IPaymentProcessor> All()
        {
            return EngineContext.Current.ResolveAll<IPaymentProcessor>();
        }

        public IPaymentProcessor FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
