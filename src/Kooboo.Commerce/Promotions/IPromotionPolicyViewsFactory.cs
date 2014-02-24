using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionPolicyViewsFactory
    {
        IPromotionPolicyViews FindByPolicyName(string policyName);
    }

    [Dependency(typeof(IPromotionPolicyViewsFactory))]
    public class PromotionPolicyViewsFactory : IPromotionPolicyViewsFactory
    {
        private IEngine _engine;
        private List<IPromotionPolicyViews> _views;

        public PromotionPolicyViewsFactory()
            : this(EngineContext.Current) { }

        public PromotionPolicyViewsFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
        }

        public IPromotionPolicyViews FindByPolicyName(string policyName)
        {
            Require.NotNullOrEmpty(policyName, "policyName");

            if (_views == null)
            {
                _views = _engine.ResolveAll<IPromotionPolicyViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.PolicyName.Equals(policyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
