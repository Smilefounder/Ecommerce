using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public abstract class CommerceAPIControllerQueryBase<T> : CommerceAPIControllerBase
    {
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            var query = BuildQueryFromQueryStrings();
            return query.ToArray();
        }

        [HttpGet]
        public virtual IEnumerable<T> Pagination(int pageIndex, int pageSize)
        {
            var query = BuildQueryFromQueryStrings();
            return query.Pagination(pageIndex, pageSize);
        }

        [HttpGet]
        public virtual T First()
        {
            var query = BuildQueryFromQueryStrings();
            return query.FirstOrDefault();
        }

        [HttpGet]
        public virtual int Count()
        {
            var query = BuildQueryFromQueryStrings();
            return query.Count();
        }
        protected abstract ICommerceQuery<T> BuildQueryFromQueryStrings();
  }
}