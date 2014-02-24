using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IExtendedQueryManager
    {
        IEnumerable<IExtendedQuery<T>> GetExtendedQueries<T>() where T : class, new();
        IExtendedQuery<T> GetExtendedQuery<T>(string name) where T : class, new();

        IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<T>(string name) where T : class, new();
        void SaveExtendedQueryParameters<T>(string name, IEnumerable<ExtendedQueryParameter> parameters) where T : class, new();
    }
}
