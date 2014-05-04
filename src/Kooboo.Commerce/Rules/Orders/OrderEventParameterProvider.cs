using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Orders
{
    [Dependency(typeof(IConditionParameterProvider), Key = "OrderEventParameterProvider")]
    public class OrderEventParameterProvider : IConditionParameterProvider
    {
        public IEnumerable<IConditionParameter> GetParameters(Type modelType)
        {
            if (typeof(IOrderEvent).IsAssignableFrom(modelType))
            {
                var result = new List<IConditionParameter>();
                var provider = new DefaultConditionParameterProvider();

                foreach (var param in provider.GetParameters(typeof(Order)))
                {
                    result.Add(new AdaptedConditionParameter(modelType, param, new OrderEventToOrderAdapter()));
                }

                return result;
            }

            return Enumerable.Empty<IConditionParameter>();
        }

        class OrderEventToOrderAdapter : IContextModelAdapter
        {
            public object AdaptModel(object rootContextModel)
            {
                var orderEvent = (IOrderEvent)rootContextModel;
                var orderId = orderEvent.OrderId;
                var orderService = EngineContext.Current.Resolve<IOrderService>();
                return orderService.GetById(orderId);
            }
        }
    }
}
