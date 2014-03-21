using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public abstract class CommerceAPIControllerAccessBase<T> : CommerceAPIControllerQueryBase<T>
    {
        [HttpPost]
        public virtual bool Post(T obj)
        {
            var accesser = GetAccesser();
            return accesser.Save(obj);
        }
        [HttpPut]
        public virtual bool Put(T obj)
        {
            var accesser = GetAccesser();
            return accesser.Update(obj);
        }
        [HttpDelete]
        public virtual bool Delete(T obj)
        {
            var accesser = GetAccesser();
            return accesser.Delete(obj);
        }

        protected override ICommerceQuery<T> BuildQueryFromQueryStrings()
        {
            throw new NotImplementedException();
        }

        protected abstract ICommerceAccess<T> GetAccesser();
    }
}
