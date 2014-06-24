using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Rules.CustomParameters
{
    // 实现 IParameterProvider 以提供额外的 Condition 参数
    public class CustomOrderParameterProvider : IParameterProvider
    {
        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
        {
            // 检查上下文类型（Event类型）是否是 IOrderEvent 的实现类
            if (!typeof(IOrderEvent).IsAssignableFrom(dataContextType))
            {
                yield break;
            }

            // 创建 ConditionParameter 实例并返回
            yield return new ConditionParameter(
                name: "Order.Customer.CustomFields.Company", // 指定参数名称
                valueType: typeof(String),  // 指定参数类型为 String
                valueResolver: ParameterValueResolver.FromDelegate(GetRemarkCustomField), // 指定参数值的获取方法
                supportedOperators: new List<IComparisonOperator>   // 指定该参数可以应用的比较符
                {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotEquals
                }
            );
        }

        // 此方法用于获取自定义条件参数的值
        private string GetRemarkCustomField(ConditionParameter parameter, object dataContext)
        {
            // 通过当前上下文获取 Order 对象
            var @event = dataContext as IOrderEvent;
            var orderId = @event.OrderId;

            var orderService = EngineContext.Current.Resolve<IOrderService>();
            var order = orderService.GetById(orderId);

            // 返回其 "Company" 这个 CustomField 的值
            var field = order.Customer.CustomFields.FirstOrDefault(f => f.Name == "Company");
            if (field == null)
            {
                return null;
            }

            return field.Value;
        }
    }
}
