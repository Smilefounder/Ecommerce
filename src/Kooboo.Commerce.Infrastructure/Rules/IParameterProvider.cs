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
    /// Defines methods to get the available parameters for a data context type.
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// Gets the available parameters of the specified data context type.
        /// </summary>
        IEnumerable<ConditionParameter> GetParameters(Type dataContextType);
    }
}
