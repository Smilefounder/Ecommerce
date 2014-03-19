using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    public class LocalPaymentQuery : LocalCommerceQuery<Payment, Kooboo.Commerce.Payments.Payment>, IPaymentQuery
    {
        private IPaymentService _paymentService;

        public LocalPaymentQuery(IPaymentService paymentService, IMapper<Payment, Kooboo.Commerce.Payments.Payment> mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        public IPaymentQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(x => x.Id == id);
            return this;
        }

        public IPaymentQuery ByTarget(string targetType, string targetId)
        {
            EnsureQuery();
            _query = _query.Where(x => x.PaymentTargetType == targetType && x.PaymentTargetId == targetId);
            return this;
        }

        protected override IQueryable<Commerce.Payments.Payment> CreateQuery()
        {
            return _paymentService.Query();
        }

        protected override IQueryable<Commerce.Payments.Payment> OrderByDefault(IQueryable<Commerce.Payments.Payment> query)
        {
            return _query.OrderByDescending(x => x.Id);
        }
    }
}
