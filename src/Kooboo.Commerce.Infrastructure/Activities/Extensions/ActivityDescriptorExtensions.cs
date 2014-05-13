using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public static class ActivityDescriptorExtensions
    {
        public static IEnumerable<IActivityDescriptor> CanBindTo(this IEnumerable<IActivityDescriptor> descriptors, Type eventType)
        {
            return descriptors.Where(x => x.CanBindTo(eventType));
        }
    }
}
