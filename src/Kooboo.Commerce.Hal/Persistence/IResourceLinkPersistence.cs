using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL.Persistence
{
    public interface IResourceLinkPersistence : IResourceLinkProvider
    {
        ResourceLink GetById(string linkId);

        void Save(ResourceLink link);

        void Delete(string linkId);
    }
}
