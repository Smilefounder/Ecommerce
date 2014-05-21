using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionPolicyProvider
    {
        IEnumerable<IPromotionPolicy> All();

        IPromotionPolicy FindByName(string policyName);
    }

    [Dependency(typeof(IPromotionPolicyProvider), ComponentLifeStyle.Singleton)]
    public class PromotionStrategyProvider : IPromotionPolicyProvider
    {
        public IEnumerable<IPromotionPolicy> All()
        {
            return EngineContext.Current.ResolveAll<IPromotionPolicy>();
        }

        public IPromotionPolicy FindByName(string policyName)
        {
            return All().FirstOrDefault(x => x.Name.Equals(policyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
