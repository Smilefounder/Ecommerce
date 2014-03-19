using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public interface IMapper<T, U>
        where T: class, new()
        where U: class, new()
    {
        T MapTo(U obj);
        U MapFrom(T obj);
    }
}
