using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Runtime
{
    public interface ITypeActivator
    {
        object Activate(Type type);
    }

    public class DefaultTypeActivator : ITypeActivator
    {
        public object Activate(Type type)
        {
            return EngineContext.Current.Resolve(type);
        }
    }
}
