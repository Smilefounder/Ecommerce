﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public static class PaymentsExtensions
    {
        public static Payment ByThirdPartyTransactionId(this IQueryable<Payment> query, string transactionId, string paymentProcessorName)
        {
            return query.FirstOrDefault(x => x.ThirdPartyTransactionId == transactionId && x.PaymentProcessorName == paymentProcessorName);
        }
    }
}
