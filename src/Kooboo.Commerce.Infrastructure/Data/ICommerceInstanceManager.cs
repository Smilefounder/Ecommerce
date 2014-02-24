using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceManager
    {
        void CreateInstance(CommerceInstanceMetadata metadata);

        void DeleteInstance(string instanceName);

        CommerceInstanceMetadata GetInstanceMetadata(string instanceName);

        IEnumerable<CommerceInstanceMetadata> GetAllInstanceMetadatas();

        CommerceInstance OpenInstance(string instanceName);
    }
}
