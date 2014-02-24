using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV.Validation
{
    public interface IValidatorFactory
    {
        IEnumerable<IValidator> AllValidators();

        IValidator FindByName(string name);
    }

    [Dependency(typeof(IValidatorFactory), ComponentLifeStyle.Singleton)]
    public class ValidatorFactory : IValidatorFactory
    {
        private IEngine _engine;

        public ValidatorFactory()
            : this(EngineContext.Current) { }

        public ValidatorFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IValidator> AllValidators()
        {
            return _engine.ResolveAll<IValidator>();
        }

        public IValidator FindByName(string name)
        {
            return AllValidators().FirstOrDefault(x => x.Name == name);
        }
    }
}
