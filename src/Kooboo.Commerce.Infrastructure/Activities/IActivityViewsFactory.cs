using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityViewsFactory
    {
        IActivityViews FindByActivityName(string activityName);
    }

    [Dependency(typeof(IActivityViewsFactory))]
    public class ActivityViewsFactory : IActivityViewsFactory
    {
        private IEngine _engine;
        private List<IActivityViews> _views;

        public ActivityViewsFactory()
            : this(EngineContext.Current) { }

        public ActivityViewsFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
        }

        public IActivityViews FindByActivityName(string activityName)
        {
            if (_views == null)
            {
                _views = _engine.ResolveAll<IActivityViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.ActivityName.Equals(activityName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
