using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Providers
{
    public interface ICommerceDatabaseOperations
    {
        void CreateDatabase(ICommerceDatabase database);

        void DeleteDatabase(ICommerceDatabase database);
    }
}
