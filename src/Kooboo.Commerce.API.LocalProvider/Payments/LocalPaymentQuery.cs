using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    public class LocalPaymentQuery : LocalCommerceQuery<Payment, Kooboo.Commerce.Payments.Payment>, IPaymentQuery
    {
        private ICommerceDatabase _database;

        public IPaymentQuery ById(int id)
        {
            _query.Where(x => x.Id == id);
            return this;
        }

        public IPaymentQuery ByTarget(string targetType, string targetId)
        {
            _query.Where(x => x.PaymentTargetType == targetType && x.PaymentTargetId == targetId);
            return this;
        }

        protected override IQueryable<Commerce.Payments.Payment> CreateQuery()
        {
            return _database.GetRepository<Kooboo.Commerce.Payments.Payment>().Query();
        }

        protected override IQueryable<Commerce.Payments.Payment> OrderByDefault(IQueryable<Commerce.Payments.Payment> query)
        {
            return _query.OrderByDescending(x => x.Id);
        }

        public override void Create(Payment obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Payment obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Payment obj)
        {
            throw new NotImplementedException();
        }
    }
}
