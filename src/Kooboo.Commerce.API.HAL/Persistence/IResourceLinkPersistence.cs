using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Persistence
{
    public interface IResourceLinkPersistence
    {
        ResourceLink GetById(string linkId);

        /// <summary>
        /// Get configured links filtering by specified environments for the specified resource
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="environmentNames">The environment names used to filter the links. Use empty set to return all available links.</param>
        /// <returns>Links for the specified resource.</returns>
        IEnumerable<ResourceLink> GetLinks(string resourceName, ISet<string> environmentNames);

        void Save(ResourceLink link);

        void Delete(string linkId);
    }
}
