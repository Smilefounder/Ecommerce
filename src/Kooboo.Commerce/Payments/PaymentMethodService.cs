using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [Dependency(typeof(PaymentMethodService))]
    public class PaymentMethodService
    {
        private CommerceInstance _instance;
        private IRepository<PaymentMethod> _repository;

        public PaymentMethodService(CommerceInstance instance)
        {
            _instance = instance;
            _repository = instance.Database.Repository<PaymentMethod>();
        }

        public PaymentMethod Find(int id)
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
                Event.Raise(new PaymentMethodEnabled(method), new EventContext(_instance));
                return true;
            }

            return false;
        }

        public bool Disable(PaymentMethod method)
        {
            if (method.MarkDisabled())
            {
                _repository.Database.SaveChanges();
                Event.Raise(new PaymentMethodDisabled(method), new EventContext(_instance));
                return true;
            }

            return false;
        }
    }
}
