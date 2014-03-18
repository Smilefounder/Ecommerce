using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.Search
{
    //[Dependency(typeof(IHttpApplicationEvents), ComponentLifeStyle.Singleton, Key="SearchAppEventHandler")]
    public class SearchAppEventHandler : HttpApplicationEvents
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            var searchProvider = EngineContext.Current.TryResolve<ISearchProvider>();
            if (searchProvider != null)
                searchProvider.Initialize();
        }

        public override void Application_End(object sender, EventArgs e)
        {
            base.Application_End(sender, e);
            var searchProvider = EngineContext.Current.TryResolve<ISearchProvider>();
            if (searchProvider != null)
                searchProvider.Close();
        }
    }
}
