using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(IPaymentAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class PaymentAPI : IPaymentAPI
    {
        public PaymentAPI()
        {
        }
    }
}
