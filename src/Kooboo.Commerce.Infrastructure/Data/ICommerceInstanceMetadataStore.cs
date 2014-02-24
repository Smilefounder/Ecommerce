using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceMetadataStore
    {
        IEnumerable<CommerceInstanceMetadata> All();

        CommerceInstanceMetadata GetByName(string name);

        void Create(CommerceInstanceMetadata metadata);

        void Update(string name, CommerceInstanceMetadata newMetadata);

        void Delete(string name);
    }
}
