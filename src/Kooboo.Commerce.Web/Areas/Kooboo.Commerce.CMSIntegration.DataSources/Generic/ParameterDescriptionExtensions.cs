using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    public static class ParameterDescriptionExtensions
    {
        public static object ResolveValue(this ParameterDescription def, string strValue)
        {
            if (String.IsNullOrWhiteSpace(strValue))
            {
                return null;
            }

            Type targetType = def.ValueType;

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }

            return Convert.ChangeType(strValue, targetType);
        }
    }
}