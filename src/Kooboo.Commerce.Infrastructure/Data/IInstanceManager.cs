using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IInstanceManager
    {
        void CreateInstance(InstanceMetadata metadata);

        void DeleteInstance(string instanceName);

        InstanceMetadata GetInstanceMetadata(string instanceName);

        IEnumerable<InstanceMetadata> GetAllInstanceMetadatas();

        CommerceInstance OpenInstance(string instanceName);
    }
}
