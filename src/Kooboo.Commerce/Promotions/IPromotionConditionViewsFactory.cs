using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionConditionViewsFactory
    {
        IPromotionConditionViews FindByConditionName(string conditionName);
    }

    [Dependency(typeof(IPromotionConditionViewsFactory))]
    public class PromotionConditionViewsFactory : IPromotionConditionViewsFactory
    {
        private IEngine _engine;
        private List<IPromotionConditionViews> _views;

        public PromotionConditionViewsFactory()
            : this(EngineContext.Current) { }

        public PromotionConditionViewsFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
        }

        public IPromotionConditionViews FindByConditionName(string conditionName)
        {
            if (_views == null)
            {
                _views = _engine.ResolveAll<IPromotionConditionViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.ConditionName.Equals(conditionName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
