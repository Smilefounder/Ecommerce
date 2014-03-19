using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API
{
    public interface ICommerceAccess<T>
    {
        bool Create(T obj);

        bool Update(T obj);

        bool Save(T obj);

        bool Delete(T obj);
    }
}
