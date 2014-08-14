using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class PaymentMethodQueryExtensions
    {
        public static Query<PaymentMethod> ById(this Query<PaymentMethod> query, int id)
        {
            return query.AddFilter(PaymentMethodFilters.ById.CreateFilter(new { Id = id }));
        }

        public static Query<PaymentMethod> ByName(this Query<PaymentMethod> query, string name)
        {
            return query.AddFilter(PaymentMethodFilters.ByName.CreateFilter(new { Name = name }));
        }

        public static Query<PaymentMethod> ByUserKey(this Query<PaymentMethod> query, string userKey)
        {
            return query.AddFilter(PaymentMethodFilters.ByUserKey.CreateFilter(new { UserKey = userKey }));
        }
    }
}
