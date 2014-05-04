using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments.Services
{
    [Dependency(typeof(IPaymentMethodService))]
    public class PaymentMethodService : IPaymentMethodService
    {
        private IRepository<PaymentMethod> _repository;

        public PaymentMethodService(IRepository<PaymentMethod> repository)
        {
            _repository = repository;
        }

        public PaymentMethod GetById(int id)
        {
            return _repository.Get(o => o.Id == id);
        }

        public IQueryable<PaymentMethod> Query()
        {
            return _repository.Query();
        }

        public bool Create(PaymentMethod method)
        {
            return _repository.Insert(method);
        }

        public bool Delete(PaymentMethod method)
        {
            return _repository.Delete(method);
        }
    }
}
