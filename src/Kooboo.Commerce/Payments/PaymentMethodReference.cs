using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [ComplexType]
    public class PaymentMethodReference
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        public string PaymentProcessorName { get; set; }

        public PaymentMethodReference() { }

        public PaymentMethodReference(PaymentMethod method)
        {
            Id = method.Id;
            DisplayName = method.DisplayName;
            PaymentProcessorName = method.PaymentProcessorName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
