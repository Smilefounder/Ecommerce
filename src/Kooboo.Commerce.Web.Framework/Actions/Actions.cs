using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Actions
{
    public static class Actions
    {
        public static IEnumerable<IEntityAction> GetActions(Type entityType)
        {
            return EngineContext.Current.ResolveAll<IEntityAction>()
                                        .Where(x => x.EntityType == entityType)
                                        .OrderBy(x => x.Order);
        }

        public static IEntityAction GetAction(Type entityType, string name)
        {
            return GetActions(entityType).FirstOrDefault(x => x.Name == name);
        }
    }
}
