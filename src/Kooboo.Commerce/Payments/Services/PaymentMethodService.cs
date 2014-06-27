using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.PaymentMethods;
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
            return _repository.Find(id);
        }

        public IQueryable<PaymentMethod> Query()
        {
            return _repository.Query();
        }

        public void Create(PaymentMethod method)
        {
            _repository.Insert(method);
        }

        public void Delete(PaymentMethod method)
        {
            _repository.Delete(method);
        }

        public bool Enable(PaymentMethod method)
        {
            if (method.MarkEnabled())
            {
                _repository.Database.SaveChanges();
                Event.Raise(new PaymentMethodEnabled(method));
                return true;
            }

            return false;
        }

        public bool Disable(PaymentMethod method)
        {
            if (method.MarkDisabled())
            {
                _repository.Database.SaveChanges();
                Event.Raise(new PaymentMethodDisabled(method));
                return true;
            }

            return false;
        }
    }
}
