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

        public IPaymentQuery By(PaymentQueryParams parameters)
        {
            if (parameters.Id != null)
            {
                ById(parameters.Id.Value);
            }
            if (parameters.Status != null)
            {
                ByStatus(parameters.Status.Value);
            }
            if (!String.IsNullOrEmpty(parameters.TargetType) && !String.IsNullOrEmpty(parameters.TargetId))
            {
                ByTarget(parameters.TargetType, parameters.TargetId);
            }

            return this;
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

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Payments.Payment> CreateQuery()
        {
            return PaymentService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Payments.Payment> OrderByDefault(IQueryable<Commerce.Payments.Payment> query)
        {
            return _query.OrderByDescending(x => x.Id);
        }

        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected override Payment Map(Commerce.Payments.Payment obj)
        {
            return _mapper.MapTo(obj);
        }
    }
}
