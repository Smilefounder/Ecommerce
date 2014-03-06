using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IListDataSourceFactory
    {
        IEnumerable<IListDataSource> GetDataSources(IParameter param);
    }
}
