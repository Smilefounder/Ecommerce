using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public interface IIndexPathBuilder
    {
        string[] GetAllPath(string directory);
        string BuildIndexPath(IDictionary<string, object> values);
        string[] BuildSearchPaths(IDictionary<string, object> values);
    }
}
