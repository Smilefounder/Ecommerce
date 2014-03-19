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

        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return _repository.Query().ToArray();
        }

        public IQueryable<PaymentMethod> Query()
        {
            return _repository.Query();
        }

        public void Enable(PaymentMethod method)
        {
            method.Enable();
        }

        public void Disable(PaymentMethod method)
        {
            method.Disable();
        }

        public bool Create(PaymentMethod method)
        {
            return _repository.Insert(method);
        }

        public bool Update(PaymentMethod method)
        {
            return _repository.Update(method, k => new object[] { k.Id });
        }

        public bool Delete(PaymentMethod method)
        {
            return _repository.Delete(method);
        }
    }
}
