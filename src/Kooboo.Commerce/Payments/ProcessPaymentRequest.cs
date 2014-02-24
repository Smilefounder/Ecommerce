using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;
using System.Web;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Payments
{
    public class ProcessPaymentRequest
    {
        public string CommerceName { get; set; }

        public Order Order { get; set; }

        public decimal Amount
        {
            get
            {
                return Order.Total;
            }
        }

        public string CurrencyCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public CreditCardInfo CreditCardInfo { get; set; }

        public string BankId { get; set; }

        public string CommerceBaseUrl { get; set; }

        public string ReturnUrl { get; set; }

        public ProcessPaymentRequest(string commerceName, Order order, PaymentMethod paymentMethod)
        {
            CommerceName = commerceName;
            Order = order;
            PaymentMethod = paymentMethod;
        }
    }
}
