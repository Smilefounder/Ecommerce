using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IImplicitLinkProvider
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<Link> GetImplicitLinks(IUriResolver uriResolver, ResourceDescriptor descriptor, IDictionary<string, object> parameters);
    }
}
