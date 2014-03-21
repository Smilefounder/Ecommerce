using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentMethodQuery))]
    public class LocalPaymentMethodQuery : LocalCommerceQuery<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod>, IPaymentMethodQuery
    {
        private IPaymentMethodService _paymentMethodService;
        private IMapper<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod> _mapper;

        public LocalPaymentMethodQuery(IPaymentMethodService paymentMethodService, IMapper<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod> mapper)
        {
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        protected override IQueryable<Commerce.Payments.PaymentMethod> CreateQuery()
        {
            return _paymentMethodService.Query().Where(x => x.IsEnabled);
        }

        protected override IQueryable<Commerce.Payments.PaymentMethod> OrderByDefault(IQueryable<Commerce.Payments.PaymentMethod> query)
        {
            return query.OrderBy(x => x.DisplayName);
        }

        protected override PaymentMethod Map(Commerce.Payments.PaymentMethod obj)
        {
            return _mapper.MapTo(obj);
        }
    }
}
