using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IHalWrapper
    {
        void AddLinks(string resourceName, IItemResource resource, HalContext context, IDictionary<string, object> parameterValues);

        void AddLinks<T>(string resourceName, IListResource<T> resource, HalContext context, IDictionary<string, object> parameterValues, Func<T, IDictionary<string, object>> itemParameterValuesResolver)
            where T : IItemResource;
    }

    public static class HalWrapperExtensions
    {
        public static void AddLinks(this IHalWrapper wrapper, string resourceName, IItemResource resource, HalContext context, object parameterValues)
        {
            wrapper.AddLinks(resourceName, resource, context, parameterValues.ToDictionary());
        }

        public static void AddLinks<T>(this IHalWrapper wrapper, string resourceName, IListResource<T> resource, HalContext context, object parameterValues, Func<T, object> itemParameterValuesResolver)
            where T : IItemResource
        {
            var convertedParamValues = parameterValues.ToDictionary();
            Func<T, IDictionary<string, object>> convertedItemParamValuesResolver = null;
            if (itemParameterValuesResolver != null)
            {
                convertedItemParamValuesResolver = item => itemParameterValuesResolver(item).ToDictionary();
            }

            wrapper.AddLinks<T>(resourceName, resource, context, convertedParamValues, convertedItemParamValuesResolver);
        }
    }
}
