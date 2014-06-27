using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Operators;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Parameters
{
    public class ParameterProviderExtensibilityFacts
    {
        [Fact]
        public void can_directly_extend_parameter_on_data_context_type()
        {
            ParameterProviders.Providers.Clear();
            ParameterProviders.Providers.Add(new DefaultParameterProvider());
            ParameterProviders.Providers.Add(new OrderParameterProvider());

            var parameters = ParameterProviders.Providers.SelectMany(p => p.GetParameters(typeof(Order))).ToList();
            var order = new Order
            {
                Id = 10,
                Total = 100,
                Subtotal = 80
            };

            var idParam = parameters.Find(p => p.Name == "Id");
            Assert.NotNull(idParam);
            Assert.Equal(10, idParam.ResolveValue(order));

            var subtotalParam = parameters.Find(p => p.Name == "Subtotal");
            Assert.NotNull(subtotalParam);
            Assert.Equal(80m, subtotalParam.ResolveValue(order));

            var discountParam = parameters.Find(p => p.Name == "Discount");
            Assert.NotNull(discountParam);
            Assert.Equal(20m, discountParam.ResolveValue(order));
        }

        [Fact]
        public void can_indirectly_extend_parameter_on_nested_object()
        {
            ParameterProviders.Providers.Clear();
            ParameterProviders.Providers.Add(new DefaultParameterProvider());
            ParameterProviders.Providers.Add(new OrderParameterProvider());

            var parameters = ParameterProviders.Providers.SelectMany(p => p.GetParameters(typeof(OrderCreated))).ToList();

            var param = parameters.Find(p => p.Name == "Order.Discount");
            Assert.NotNull(param);

            var value = param.ResolveValue(new OrderCreated
            {
                Order = new Order
                {
                    Total = 100m,
                    Subtotal = 90m
                }
            });

            Assert.Equal(10m, value);
        }

        [Fact]
        public void can_indirectly_extend_parameter_on_deeply_nested_object()
        {
            ParameterProviders.Providers.Clear();
            ParameterProviders.Providers.Add(new DefaultParameterProvider());
            ParameterProviders.Providers.Add(new OrderParameterProvider());

            var param = ParameterProviders.Providers
                                          .SelectMany(p => p.GetParameters(typeof(PaymentCreated)))
                                          .ToList()
                                          .Find(p => p.Name == "Payment.Order.Discount");

            Assert.NotNull(param);

            var value = param.ResolveValue(new PaymentCreated
            {
                Payment = new Payment
                {
                    Id = 1,
                    Order = new Order
                    {
                        Id = 5,
                        Total = 250,
                        Subtotal = 200
                    }
                }
            });

            Assert.Equal(50m, value);
        }

        public class OrderParameterProvider : IParameterProvider
        {
            public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
            {
                if (dataContextType != typeof(Order))
                {
                    yield break;
                }

                yield return new ConditionParameter(
                    name: "Discount",
                    valueType: typeof(decimal),
                    valueResolver: ParameterValueResolver.FromDelegate((p, dataContext) =>
                    {
                        var order = (Order)dataContext;
                        return order.Total - order.Subtotal;
                    }),
                    supportedOperators: ComparisonOperators.Operators
                );
            }
        }

        #region Models

        public class OrderCreated
        {
            [Reference]
            public Order Order { get; set; }
        }

        public class PaymentCreated
        {
            [Reference]
            public Payment Payment { get; set; }
        }

        public class Payment
        {
            public int Id { get; set; }

            [Reference]
            public Order Order { get; set; }
        }

        public class Order
        {
            [Param]
            public int Id { get; set; }

            [Param]
            public decimal Subtotal { get; set; }

            [Param]
            public decimal Total { get; set; }
        }

        #endregion
    }
}
