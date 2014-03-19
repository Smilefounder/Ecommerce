using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public abstract class LocalCommerceQueryAccess<T, Model> : LocalCommerceQuery<T, Model>, ICommerceAccess<T>
        where T : class, new()
        where Model : class, new()
    {
        public abstract bool Create(T obj);

        public abstract bool Update(T obj);

        public abstract bool Save(T obj);

        public abstract bool Delete(T obj);
    }
}
