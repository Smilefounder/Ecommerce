using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ShippingMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping.Services
{
    [Dependency(typeof(IShippingMethodService))]
    public class ShippingMethodService : IShippingMethodService
    {
        private IRepository<ShippingMethod> _repository;

        public ShippingMethodService(IRepository<ShippingMethod> repository)
        {
            _repository = repository;
        }

        public ShippingMethod GetById(int id)
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

            Event.Raise(new ShippingMethodEnabled(method));

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

            Event.Raise(new ShippingMethodDisabled(method));

            return true;
        }
    }
}
