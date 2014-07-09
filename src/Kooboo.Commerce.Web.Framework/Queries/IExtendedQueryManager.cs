using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IExtendedQueryManager
    {
        IEnumerable<EQ> GetExtendedQueries<EQ>()
            where EQ : IExtendedQuery;
        EQ GetExtendedQuery<EQ>(string name)
            where EQ : IExtendedQuery;

        IEnumerable<ExtendedQueryParameter> GetExtendedQueryParameters<EQ>(string name)
            where EQ : IExtendedQuery;
        void SaveExtendedQueryParameters<EQ>(string name, IEnumerable<ExtendedQueryParameter> parameters)
            where EQ : IExtendedQuery;
    }
}
