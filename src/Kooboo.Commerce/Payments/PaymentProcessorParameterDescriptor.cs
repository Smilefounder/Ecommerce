using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class PaymentProcessorParameterDescriptor
    {
        public string ParameterName { get; set; }

        public bool IsRequired { get; set; }

        public string Description { get; set; }

        public PaymentProcessorParameterDescriptor(string parameterName)
        {
            ParameterName = parameterName;
            IsRequired = true;
        }
    }
}
