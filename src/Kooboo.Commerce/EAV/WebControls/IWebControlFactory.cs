using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV.WebControls
{
    public interface IWebControlFactory
    {
        IEnumerable<IWebControl> AllWebControls();

        IWebControl FindByName(string controlName);
    }

    [Dependency(typeof(IWebControlFactory), ComponentLifeStyle.Singleton)]
    public class WebControlFactory : IWebControlFactory
    {
        private IEngine _engine;

        public WebControlFactory()
            : this(EngineContext.Current) { }

        public WebControlFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IWebControl> AllWebControls()
        {
            return _engine.ResolveAll<IWebControl>();
        }

        public IWebControl FindByName(string controlName)
        {
            return AllWebControls().FirstOrDefault(x => x.Name == controlName);
        }
    }

}
