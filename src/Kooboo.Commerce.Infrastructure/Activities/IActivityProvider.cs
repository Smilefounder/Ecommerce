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
        IEnumerable<IActivity> All();

        IActivity FindByName(string name);

        IEnumerable<IActivity> FindBindableTo(Type eventType);
    }

    [Dependency(typeof(IActivityProvider), ComponentLifeStyle.Singleton)]
    public class DefaultActivityProvider : IActivityProvider
    {
        public IEnumerable<IActivity> All()
        {
            return EngineContext.Current.ResolveAll<IActivity>();
        }

        public IActivity FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<IActivity> FindBindableTo(Type eventType)
        {
            return All().Where(x => x.CanBindTo(eventType));
        }
    }
}
