using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Tests.Common.Data;
using Kooboo.Commerce.Tests.Common.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Kooboo.Commerce.Tests.Orders
{
    public class OrderServiceFacts
    {
        public class Create
        {
            [Fact]
            public void should_raise_order_created_event()
            {
                using (var scope = EventTrackingScope.Begin())
                {
                    var database = new MockDatabase(new CommerceInstanceSettings());
                    var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                    var service = new OrderService(instance);
                    service.Create(new Order
                    {
                        Total = 100
                    });

                    Assert.Equal(1, scope.TotalRaisingTimes<OrderCreated>());
                    // Order creation should not be treated as "change order status to Created"
                    Assert.Equal(0, scope.TotalRaisingTimes<OrderStatusChanged>());
                }
            }
        }

        public class AcceptPaymentResult
        {
            [Fact]
            public void should_ignore_subsequence_payment_result_once_succeeded()
            {
                var database = new MockDatabase(new CommerceInstanceSettings());
                var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                database.Repository<Order>().Items.Add(11, new Order { Id = 11, Total = 50 });

                var service = new OrderService(instance);
                var payment = new Payment
                {
                    Id = 1,
                    OrderId = 11,
                    Amount = 50,
                    PaymentMethodId = 1,
                    Status = PaymentStatus.Pending
                };

                // Success payment
                using (var scope = EventTrackingScope.Begin())
                {
                    service.AcceptPaymentProcessResult(payment, new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    });

                    Assert.Equal(PaymentStatus.Success, payment.Status);
                    Assert.Equal(1, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Failure payment
                using (var host = EventTrackingScope.Begin())
                {
                    service.AcceptPaymentProcessResult(payment, new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Failed
                    });

                    Assert.Equal(PaymentStatus.Success, payment.Status);
                    Assert.Equal(0, host.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Cancelled payment
                using (var scope = EventTrackingScope.Begin())
                {
                    service.AcceptPaymentProcessResult(payment, new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Cancelled
                    });
                    Assert.Equal(PaymentStatus.Success, payment.Status);
                    Assert.Equal(0, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Still pending payment
                using (var scope = EventTrackingScope.Begin())
                {
                    service.AcceptPaymentProcessResult(payment, new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Pending
                    });
                    Assert.Equal(PaymentStatus.Success, payment.Status);
                    Assert.Equal(0, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }
            }
        }

        public class ProcessPayment
        {
            [Fact]
            public void should_append_payment_method_cost()
            {
                var settings = new CommerceInstanceSettings();
                var database = new MockDatabase(settings);
                database.Repository<Order>().Items.Add(1, new Order());

                var service = new OrderService(new CommerceInstance(settings, _ => database))
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                var method = new PaymentMethod
                {
                    AdditionalFeeChargeMode = PaymentMethodFeeChargeMode.ByAmount,
                    AdditionalFeeAmount = 5
                };

                var result = service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 100,
                    PaymentMethod = method
                });

                var payment = service.Payments().ById(result.PaymentId);

                Assert.Equal(105, payment.Amount);
                Assert.Equal(5, payment.PaymentMethodCost);
            }

            [Fact]
            public void should_raise_payment_status_changed_event()
            {
                var settings = new CommerceInstanceSettings();
                var database = new MockDatabase(settings);
                database.Repository<Order>().Items.Add(1, new Order { Id = 1, Total = 250 });

                var method = new PaymentMethod();

                // Success payments
                var service = new OrderService(new CommerceInstance(settings, _ => database))
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                using (var scope = EventTrackingScope.Begin())
                {
                    scope.Host.Listen<PaymentStatusChanged>((@event, ctx) =>
                    {
                        Assert.Equal(1, @event.OrderId);
                        Assert.Equal(PaymentStatus.Pending, @event.OldStatus);
                        Assert.Equal(PaymentStatus.Success, @event.NewStatus);
                    });

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 100,
                        PaymentMethod = method
                    });
                    Assert.Equal(1, scope.TotalRaisingTimes<PaymentStatusChanged>());

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 100,
                        PaymentMethod = method
                    });
                    Assert.Equal(2, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Failure payments
                service.GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                {
                    PaymentStatus = PaymentStatus.Failed
                });

                using (var scope = EventTrackingScope.Begin())
                {
                    scope.Host.Listen<PaymentStatusChanged>((@event, ctx) =>
                    {
                        Assert.Equal(PaymentStatus.Pending, @event.OldStatus);
                        Assert.Equal(PaymentStatus.Failed, @event.NewStatus);
                    });

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 50,
                        PaymentMethod = method
                    });
                    Assert.Equal(1, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Cancelled payments
                service.GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                {
                    PaymentStatus = PaymentStatus.Cancelled
                });

                using (var scope = EventTrackingScope.Begin())
                {
                    scope.Host.Listen<PaymentStatusChanged>((@event, ctx) =>
                    {
                        Assert.Equal(PaymentStatus.Cancelled, @event.NewStatus);
                    });

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 50,
                        PaymentMethod = method
                    });
                    Assert.Equal(1, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }

                // Don't raise events if it's still pending
                service.GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                {
                    PaymentStatus = PaymentStatus.Pending
                });

                using (var scope = EventTrackingScope.Begin())
                {
                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 50,
                        PaymentMethod = method
                    });
                    Assert.Equal(0, scope.TotalRaisingTimes<PaymentStatusChanged>());
                }
            }

            [Fact]
            public void should_raise_order_status_changed_event_when_all_paid()
            {
                var database = new MockDatabase(new CommerceInstanceSettings());
                var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                database.Repository<Order>().Items.Add(1, new Order { Id = 1, Total = 100 });

                var service = new OrderService(instance)
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                using (var host = EventTrackingScope.Begin())
                {
                    host.Host.Listen<OrderStatusChanged>((@event, ctx) =>
                    {
                        Assert.Equal(OrderStatus.Created, @event.OldStatus);
                        Assert.Equal(OrderStatus.Paid, @event.NewStatus);
                    });

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 80,
                        PaymentMethod = new PaymentMethod()
                    });

                    Assert.Equal(0, EventTrackingScope.Current.TotalRaisingTimes<OrderStatusChanged>());

                    service.ProcessPayment(new PaymentRequest
                    {
                        OrderId = 1,
                        Amount = 20,
                        PaymentMethod = new PaymentMethod()
                    });

                    Assert.Equal(1, EventTrackingScope.Current.TotalRaisingTimes<OrderStatusChanged>());
                };
            }

            [Fact]
            public void should_update_order_info_after_success()
            {
                var database = new MockDatabase(new CommerceInstanceSettings());
                var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                database.Repository<Order>().Items.Add(1, new Order { Id = 1, Total = 100 });

                var service = new OrderService(instance)
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 100,
                    PaymentMethod = new PaymentMethod()
                });

                var order = database.Repository<Order>().Find(1);
                Assert.Equal(100, order.TotalPaid);
                Assert.Equal(OrderStatus.Paid, order.Status);
            }

            [Fact]
            public void should_allow_multi_payments()
            {
                var database = new MockDatabase(new CommerceInstanceSettings());
                var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                database.Repository<Order>().Items.Add(1, new Order { Id = 1, Total = 100 });

                var service = new OrderService(instance)
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 50,
                    PaymentMethod = new PaymentMethod()
                });

                var order = database.Repository<Order>().Find(1);
                Assert.Equal(50, order.TotalPaid);
                Assert.Equal(OrderStatus.Created, order.Status);

                service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 20,
                    PaymentMethod = new PaymentMethod()
                });

                order = database.Repository<Order>().Find(1);
                Assert.Equal(70, order.TotalPaid);
                Assert.Equal(OrderStatus.Created, order.Status);

                service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 30,
                    PaymentMethod = new PaymentMethod()
                });

                order = database.Repository<Order>().Find(1);
                Assert.Equal(100, order.TotalPaid);
                Assert.Equal(OrderStatus.Paid, order.Status);
            }

            [Fact]
            public void should_record_over_pay()
            {
                var database = new MockDatabase(new CommerceInstanceSettings());
                var instance = new CommerceInstance(new CommerceInstanceSettings(), _ => database);

                database.Repository<Order>().Items.Add(1, new Order { Id = 1, Total = 100 });

                var service = new OrderService(instance)
                {
                    GetPaymentProcessorByName = _ => MockPaymentProcessor(__ => new PaymentProcessResult
                    {
                        PaymentStatus = PaymentStatus.Success
                    })
                };

                service.ProcessPayment(new PaymentRequest
                {
                    OrderId = 1,
                    Amount = 115,
                    PaymentMethod = new PaymentMethod()
                });

                var order = database.Repository<Order>().Find(1);
                Assert.Equal(115, order.TotalPaid);
                Assert.Equal(OrderStatus.Paid, order.Status);
            }
        }

        static IPaymentProcessor MockPaymentProcessor(Func<PaymentProcessingContext, PaymentProcessResult> process)
        {
            var processorMock = new Mock<IPaymentProcessor>();
            processorMock.Setup(it => it.ConfigType).Returns<Type>(null);
            processorMock.Setup(it => it.Process(It.IsAny<PaymentProcessingContext>()))
                         .Returns(process);

            return processorMock.Object;
        }
    }
}
