using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods
{
    public class PaymentProcessorConfigEditorModel
    {
        public int PaymentMethodId { get; set; }

        public object Config { get; set; }
    }
}