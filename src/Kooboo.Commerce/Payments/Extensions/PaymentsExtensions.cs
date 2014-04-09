using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public static class PaymentsExtensions
    {
        public static IQueryable<Payment> ByTarget(this IQueryable<Payment> query, string targetType, string targetId)
        {
            return query.Where(x => x.PaymentTarget.Type == targetType && x.PaymentTarget.Id == targetId);
        }

        public static IQueryable<Payment> ByTargetType(this IQueryable<Payment> query, string targetType)
        {
            return query.Where(x => x.PaymentTarget.Type == targetType);
        }

        public static IQueryable<Payment> ForOrders(this IQueryable<Payment> query)
        {
            return query.ByTargetType(PaymentTargetTypes.Order);
        }

        public static IQueryable<Payment> WhereSucceeded(this IQueryable<Payment> query)
        {
            return query.Where(x => x.Status == PaymentStatus.Success);
        }

        public static Payment ByThirdPartyTransactionId(this IQueryable<Payment> query, string transactionId, string paymentProcessorName)
        {
            return query.FirstOrDefault(x => x.ThirdPartyTransactionId == transactionId && x.PaymentMethod.PaymentProcessorName == paymentProcessorName);
        }
    }
}
