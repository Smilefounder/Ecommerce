using Kooboo.CMS.Common.Runtime.Dependency;
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
        protected IPaymentService PaymentService { get; private set; }
        private IMapper<Payment, Kooboo.Commerce.Payments.Payment> _mapper;

        public LocalPaymentQuery(IPaymentService paymentService, IMapper<Payment, Kooboo.Commerce.Payments.Payment> mapper)
        {
            PaymentService = paymentService;
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

        public IPaymentQuery ByStatus(PaymentStatus status)
        {
            EnsureQuery();
            var mappedStatus = (Kooboo.Commerce.Payments.PaymentStatus)Enum.Parse(typeof(PaymentStatus), status.ToString());
            _query = _query.Where(x => x.Status == mappedStatus);
            return this;
        }

        public IPaymentQuery OrderById(SortDirection direction)
        {
            EnsureQuery();

            if (direction == SortDirection.Asc)
            {
                _query = _query.OrderBy(x => x.Id);
            }
            else
            {
                _query = _query.OrderByDescending(x => x.Id);
            }

            return this;
        }

        protected override IQueryable<Commerce.Payments.Payment> CreateQuery()
        {
            return PaymentService.Query();
        }

        protected override IQueryable<Commerce.Payments.Payment> OrderByDefault(IQueryable<Commerce.Payments.Payment> query)
        {
            return _query.OrderByDescending(x => x.Id);
        }

        protected override Payment Map(Commerce.Payments.Payment obj)
        {
            return _mapper.MapTo(obj);
        }
    }
}
