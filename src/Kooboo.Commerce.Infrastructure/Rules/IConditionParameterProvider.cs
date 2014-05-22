using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Provides available condition expression parameters for a model type.
    /// </summary>
    public interface IConditionParameterProvider
    {
        /// <summary>
        /// Get the available condition expression parameters for the specified model type.
        /// </summary>
        IEnumerable<ConditionParameter> GetParameters(Type dataContextType);
    }
}
