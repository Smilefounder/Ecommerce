using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Persistence
{
    public interface IResourceLinkPersistence
    {
        ResourceLink GetById(string linkId);

        IEnumerable<ResourceLink> GetLinks(string resourceName);

        void Save(ResourceLink link);

        void Delete(string linkId);
    }
}
