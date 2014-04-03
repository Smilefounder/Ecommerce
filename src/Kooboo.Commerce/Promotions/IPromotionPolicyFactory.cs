using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionPolicyFactory
    {
        IEnumerable<IPromotionPolicy> All();

        IPromotionPolicy FindByName(string policyName);
    }

    [Dependency(typeof(IPromotionPolicyFactory), ComponentLifeStyle.Singleton)]
    public class PromotionStrategyFactory : IPromotionPolicyFactory
    {
        private IEngine _engine;

        public PromotionStrategyFactory()
            : this(EngineContext.Current)
        {
        }

        public PromotionStrategyFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IPromotionPolicy> All()
        {
            return _engine.ResolveAll<IPromotionPolicy>();
        }

        public IPromotionPolicy FindByName(string strategyName)
        {
            return All().FirstOrDefault(x => x.Name == strategyName);
        }
    }
}
