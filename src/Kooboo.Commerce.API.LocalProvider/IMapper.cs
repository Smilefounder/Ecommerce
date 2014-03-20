using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public interface IMapper<T, U>
        where T: class, new()
        where U: class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="includeComplexPropertyNames">include extra complex type property</param>
        /// <returns></returns>
        T MapTo(U obj, params string[] includeComplexPropertyNames);
        U MapFrom(T obj, params string[] includeComplexPropertyNames);
    }
}
