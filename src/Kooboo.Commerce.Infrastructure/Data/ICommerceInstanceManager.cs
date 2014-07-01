﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceManager
    {
        IEnumerable<CommerceInstance> GetInstances();

        CommerceInstance GetInstance(string instanceName);

        CommerceInstanceMetadata GetMetadata(string instanceName);

        void CreateInstance(CommerceInstanceMetadata metadata);

        void DeleteInstance(string instanceName);
    }
}
