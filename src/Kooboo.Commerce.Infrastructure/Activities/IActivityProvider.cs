using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityProvider
    {
        IEnumerable<IActivityDescriptor> GetAllDescriptors();

        IActivityDescriptor GetDescriptorFor(string activityName);
    }

    [Dependency(typeof(IActivityProvider), ComponentLifeStyle.Singleton)]
    public class DefaultActivityProvider : IActivityProvider
    {
        private Lazy<Dictionary<string, IActivityDescriptor>> _descriptorsByNames;

        public DefaultActivityProvider()
        {
            _descriptorsByNames = new Lazy<Dictionary<string, IActivityDescriptor>>(LoadDescriptorsByNames);
        }

        public IEnumerable<IActivityDescriptor> GetAllDescriptors()
        {
            return _descriptorsByNames.Value.Values.ToList();
        }

        public IActivityDescriptor GetDescriptorFor(string activityName)
        {
            IActivityDescriptor descriptor;

            if (_descriptorsByNames.Value.TryGetValue(activityName, out descriptor))
            {
                return descriptor;
            }

            return null;
        }

        private Dictionary<string, IActivityDescriptor> LoadDescriptorsByNames()
        {
            return EngineContext.Current
                                .ResolveAll<IActivityDescriptor>()
                                .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
        }
    }
}
