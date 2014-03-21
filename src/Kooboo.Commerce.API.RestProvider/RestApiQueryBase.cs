using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider
{
    public class RestApiQueryBase<T> : RestApiBase, ICommerceQuery<T>
    {
        public virtual T[] Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<T[]>("Pagination");
        }

        public virtual T FirstOrDefault()
        {
            return Get<T>("First");
        }

        public virtual T[] ToArray()
        {
            return Get<T[]>(null);
        }

        public virtual int Count()
        {
            return Get<int>("Count");
        }

        protected override string ApiControllerPath
        {
            get
            {
                return typeof(T).Name;
            }
        }
    }
}
