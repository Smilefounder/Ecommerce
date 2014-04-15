using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Hal.Persistence
{
    public class FileResourceLinkPersitence : IResourceLinkPersitence
    {
        public void Save(ResourceLink link)
        {
            throw new NotImplementedException();
        }

        public void Delete(string linkId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResourceLink> GetLinks(string resourceName)
        {
            throw new NotImplementedException();
        }
    }
}
