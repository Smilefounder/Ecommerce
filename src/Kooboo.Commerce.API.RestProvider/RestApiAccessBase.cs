using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider
{
    public class RestApiAccessBase<T> : RestApiQueryBase<T>, ICommerceAccess<T>
    {
        public virtual bool Create(T obj)
        {
            return Post<bool>(null, obj);
        }

        public virtual bool Update(T obj)
        {
            return Put<bool>(null, obj);
        }

        public virtual bool Save(T obj)
        {
            return Post<bool>(null, obj);
        }

        public virtual bool Delete(T obj)
        {
            return Delete<bool>(null, obj);
        }
    }
}
