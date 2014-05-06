using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL.Serialization
{
    static class ResourceTypeUtil
    {
        public static bool IsLinkCollection(Type type)
        {
            return typeof(IEnumerable<Link>).IsAssignableFrom(type);
        }

        public static bool IsResourceType(Type type)
        {
            return typeof(IResource).IsAssignableFrom(type);
        }

        public static bool IsResourceCollection(Type type)
        {
            return typeof(IEnumerable<IResource>).IsAssignableFrom(type);
        }
    }
}
