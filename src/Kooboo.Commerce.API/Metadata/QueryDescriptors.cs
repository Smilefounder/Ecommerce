using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.API.Metadata
{
    public static class QueryDescriptors
    {
        static readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        static readonly Dictionary<string, QueryDescriptor> _descriptorsByNames = new Dictionary<string, QueryDescriptor>();

        public static IEnumerable<QueryDescriptor> Descriptors
        {
            get
            {
                _readWriteLock.EnterReadLock();

                try
                {
                    return _descriptorsByNames.Values.ToList();
                }
                finally
                {
                    _readWriteLock.ExitReadLock();
                }
            }
        }

        static QueryDescriptors()
        {
            RegisterAssemblies(typeof(QueryDescriptors).Assembly);
        }

        public static void RegisterAssemblies(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(asm => asm.GetExportedTypes()).ToList();

            _readWriteLock.EnterWriteLock();

            try
            {
                foreach (var type in types)
                {
                    QueryDescriptor descriptor;
                    if (QueryDescriptor.TryDescribe(type, out descriptor))
                    {
                        if (_descriptorsByNames.ContainsKey(descriptor.Name))
                            throw new InvalidOperationException("A query with name '" + descriptor.Name + "' has already been registered. Ensure each query have a unique name. Checking type: " + type + ".");

                        _descriptorsByNames.Add(descriptor.Name, descriptor);
                    }
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public static QueryDescriptor GetDescriptor(string name)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                QueryDescriptor descriptor;
                if (_descriptorsByNames.TryGetValue(name, out descriptor))
                {
                    return descriptor;
                }

                return null;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }
    }
}
