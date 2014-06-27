using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface IInstanceMetadataStore
    {
        IEnumerable<InstanceMetadata> All();

        InstanceMetadata GetByName(string name);

        void Create(InstanceMetadata metadata);

        void Update(string name, InstanceMetadata newMetadata);

        void Delete(string name);
    }
}
