using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IHalWrapper
    {
        void AddLinks(string resourceName, IItemResource resource, IDictionary<string, object> parameterValues);

        void AddLinks<T>(string resourceName, IListResource<T> resource, IDictionary<string, object> parameterValues, Func<T, IDictionary<string, object>> itemParameterValuesResolver)
            where T : IItemResource;
    }

    public static class HalWrapperExtensions
    {
        public static void AddLinks(this IHalWrapper wrapper, string resourceName, IItemResource resource, object parameterValues)
        {
            wrapper.AddLinks(resourceName, resource, parameterValues.ToDictionary());
        }

        public static void AddLinks<T>(this IHalWrapper wrapper, string resourceName, IListResource<T> resource, object parameterValues, Func<T, object> itemParameterValuesResolver)
            where T : IItemResource
        {
            var convertedParamValues = parameterValues.ToDictionary();
            Func<T, IDictionary<string, object>> convertedItemParamValuesResolver = null;
            if (itemParameterValuesResolver != null)
            {
                convertedItemParamValuesResolver = item => itemParameterValuesResolver(item).ToDictionary();
            }

            wrapper.AddLinks<T>(resourceName, resource, convertedParamValues, convertedItemParamValuesResolver);
        }
    }
}
