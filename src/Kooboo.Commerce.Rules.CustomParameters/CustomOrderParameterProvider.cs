using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Rules.Conditions.Operators;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Rules.CustomParameters
{
    public class CustomOrderParameterProvider : IRuleParameterProvider
    {
        public IEnumerable<RuleParameter> GetParameters(Type dataContextType)
        {
            if (dataContextType != typeof(Order))
            {
                yield break;
            }

            // 创建 ConditionParameter 实例并返回
            yield return new RuleParameter(
                name: "Customer.Company", // 指定参数名称
                valueType: typeof(String),  // 指定参数类型为 String
                valueResolver: RuleParameterValueResolver.FromDelegate(GetCustomerCompnay), // 指定参数值的获取方法
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
        private string GetCustomerCompnay(RuleParameter parameter, object dataContext)
        {
            var order = (Order)dataContext;

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
