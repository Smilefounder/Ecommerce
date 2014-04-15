using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Hal.Persistence
{
    public interface IResourceLinkPersitence : IResourceLinkProvider
    {
        void Save(ResourceLink link);

        void Delete(string linkId);
    }
}
