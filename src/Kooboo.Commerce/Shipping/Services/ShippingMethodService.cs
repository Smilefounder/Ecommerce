using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
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
            return _repository.Get(o => o.Id == id);
        }

        public IQueryable<ShippingMethod> Query()
        {
            return _repository.Query();
        }

        public void Create(ShippingMethod method)
        {
            _repository.Insert(method);
        }

        public void Update(ShippingMethod method)
        {
            _repository.Update(method, k => new object[] { k.Id });
        }

        public void Delete(ShippingMethod method)
        {
            _repository.Delete(method);
        }

        public void Enable(ShippingMethod method)
        {
            method.Enable();
        }

        public void Disable(ShippingMethod method)
        {
            method.Disable();
        }
    }
}
