using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceManager
    {
        IEnumerable<CommerceInstance> GetInstances();

        CommerceInstance GetInstance(string instanceName);

        CommerceInstanceSettings GetInstanceSettings(string instanceName);

        void CreateInstance(CommerceInstanceSettings settings);

        void DeleteInstance(string instanceName);
    }
}
