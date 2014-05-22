using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class PropertyBackedParameterValueResolver : IParameterValueResolver
    {
        private PropertyInfo _property;

        public PropertyBackedParameterValueResolver(PropertyInfo property)
        {
            _property = property;
        }

        public object GetValue(ConditionParameter param, object dataContext)
        {
            if (dataContext == null)
            {
                return null;
            }

            return _property.GetValue(dataContext, null);
        }
    }
}
