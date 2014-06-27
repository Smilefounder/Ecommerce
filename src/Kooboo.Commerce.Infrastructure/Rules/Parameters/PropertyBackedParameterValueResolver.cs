using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// Represents a resolver to resolve the value of a class property backed parameter.
    /// </summary>
    public class PropertyBackedParameterValueResolver : ParameterValueResolver
    {
        private PropertyInfo _property;

        public PropertyBackedParameterValueResolver(PropertyInfo property)
        {
            Require.NotNull(property, "property");
            _property = property;
        }

        public override object ResolveValue(ConditionParameter param, object dataContext)
        {
            if (dataContext == null)
            {
                return null;
            }

            return _property.GetValue(dataContext, null);
        }
    }
}
