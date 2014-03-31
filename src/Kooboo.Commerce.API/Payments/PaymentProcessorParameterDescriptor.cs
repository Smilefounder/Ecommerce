using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    /// <summary>
    /// Describes the metadata information of a payment processor parameter.
    /// </summary>
    public class PaymentProcessorParameterDescriptor
    {
        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets if this parameter is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the description of this parameter.
        /// </summary>
        public string Description { get; set; }
    }
}
