using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [ComplexType]
    public class PaymentMethodInfo
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string ProcessorName { get; set; }

        public PaymentMethodInfo() { }

        public PaymentMethodInfo(PaymentMethod method)
        {
            Id = method.Id;
            Name = method.Name;
            ProcessorName = method.PaymentProcessorName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
