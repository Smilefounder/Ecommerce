using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public interface IMapper<T, U>
    {
        T MapTo(U obj);
        U MapFrom(T obj);
    }
}
