using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(IPaymentAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class PaymentAPI : RestApiBase, IPaymentAPI
    {
        public PaymentAPI()
        {
        }

        protected override string ApiControllerPath
        {
            get { return "Payment"; }
        }
    }
}
