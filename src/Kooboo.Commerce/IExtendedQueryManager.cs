using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IExtendedQueryManager
    {
        IEnumerable<IExtendedQuery<T, Q>> GetExtendedQueries<T, Q>()
            where T : class, new()
            where Q : class, new();
        IExtendedQuery<T, Q> GetExtendedQuery<T, Q>(string name)
            where T : class, new()
            where Q : class, new();

        IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<T, Q>(string name)
            where T : class, new()
            where Q : class, new();
        void SaveExtendedQueryParameters<T, Q>(string name, IEnumerable<ExtendedQueryParameter> parameters)
            where T : class, new()
            where Q : class, new();
    }
}
