using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionConditionFactory
    {
        IEnumerable<IPromotionCondition> All();

        IPromotionCondition FindByName(string name);
    }

    [Dependency(typeof(IPromotionConditionFactory), ComponentLifeStyle.Singleton)]
    public class PromotionConditionFactory : IPromotionConditionFactory
    {
        private IEngine _engine;

        public PromotionConditionFactory()
            : this(EngineContext.Current)
        {
        }

        public PromotionConditionFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IPromotionCondition> All()
        {
            return _engine.ResolveAll<IPromotionCondition>();
        }

        public IPromotionCondition FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name == name);
        }
    }
}
