using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ShippingMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    [Dependency(typeof(ShippingMethodService))]
    public class ShippingMethodService
    {
        private CommerceInstance _instance;
        private IRepository<ShippingMethod> _repository;

        public ShippingMethodService(CommerceInstance instance)
        {
            _instance = instance;
            _repository = _instance.Database.Repository<ShippingMethod>();
        }

        public ShippingMethod Find(int id)
        {
            return _repository.Find(id);
        }

        public IQueryable<ShippingMethod> Query()
        {
            return _repository.Query();
        }

        public void Create(ShippingMethod method)
        {
            _repository.Insert(method);
        }

        public void Delete(ShippingMethod method)
        {
            if (method.IsEnabled)
            {
                Disable(method);
            }

            _repository.Delete(method);
        }

        public bool Enable(ShippingMethod method)
        {
            if (method.IsEnabled)
            {
                return false;
            }

            method.IsEnabled = true;
            _repository.Database.SaveChanges();

            Event.Raise(new ShippingMethodEnabled(method), _instance);

            return true;
        }

        public bool Disable(ShippingMethod method)
        {
            if (!method.IsEnabled)
            {
                return false;
            }

            method.IsEnabled = false;
            _repository.Database.SaveChanges();

            Event.Raise(new ShippingMethodDisabled(method), _instance);

            return true;
        }
    }
}
