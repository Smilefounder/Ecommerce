using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    public static class QueryDescriptors
    {
        static readonly Dictionary<string, QueryDescriptor> _descriptorsByNames = new Dictionary<string, QueryDescriptor>();

        public static IEnumerable<QueryDescriptor> Descriptors
        {
            get
            {
                return _descriptorsByNames.Values;
            }
        }

        static QueryDescriptors()
        {
            var assembly = typeof(QueryDescriptor).Assembly;
            foreach (var type in assembly.GetExportedTypes())
            {
                QueryDescriptor descriptor;
                if (QueryDescriptor.TryDescribe(type, out descriptor))
                {
                    if (_descriptorsByNames.ContainsKey(descriptor.Name))
                        throw new InvalidOperationException("A query with name '" + descriptor.Name + "' has already been registered. Ensure each query have a unique name.");

                    _descriptorsByNames.Add(descriptor.Name, descriptor);
                }
            }
        }

        public static QueryDescriptor GetDescriptor(string name)
        {
            QueryDescriptor descriptor;
            if (_descriptorsByNames.TryGetValue(name, out descriptor))
            {
                return descriptor;
            }

            return null;
        }
    }
}
