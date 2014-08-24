using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public interface IItemsReader
    {
        IEnumerable<string> GetItems();
    }
}