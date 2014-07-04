using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions.Operators;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Parameters
{
    public class DefaultParameterProviderFacts
    {
        public class EnumParameter
        {
            [Fact]
            public void can_discover_enum()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextModel)).ToList();

                var param = parameters.Find(p => p.Name == "EnumValue");
                Assert.NotNull(param);
                Assert.NotNull(param.ValueSource);

                var values = param.ValueSource.GetValues(param);
                Assert.Equal(3, values.Count());
                Assert.Equal("Value1", values["Value1"]);
                Assert.Equal("Value2", values["Value2"]);
                Assert.Equal("Value3", values["Value3"]);
            }

            [Fact]
            public void can_discover_nullable_enum()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextModel)).ToList();

                var param = parameters.Find(p => p.Name == "NullableEnumValue");
                Assert.NotNull(param);
                Assert.NotNull(param.ValueSource);

                var values = param.ValueSource.GetValues(param);
                Assert.Equal(3, values.Count());
                Assert.Equal("Value1", values["Value1"]);
                Assert.Equal("Value2", values["Value2"]);
                Assert.Equal("Value3", values["Value3"]);
            }

            [Fact]
            public void can_resolve_enum_value()
            {
                var provider = new DefaultRuleParameterProvider();
                var param = provider.GetParameters(typeof(ContextModel))
                                    .FirstOrDefault(p => p.Name == "EnumValue");

                Assert.Equal(MyEnum.Value2, param.ResolveValue(new ContextModel
                {
                    EnumValue = MyEnum.Value2
                }));

                Assert.Equal(MyEnum.Value3, param.ResolveValue(new ContextModel
                {
                    EnumValue = MyEnum.Value3
                }));
            }

            [Fact]
            public void can_resolve_nullable_enum_value()
            {
                var provider = new DefaultRuleParameterProvider();
                var param = provider.GetParameters(typeof(ContextModel))
                                    .FirstOrDefault(p => p.Name == "NullableEnumValue");

                Assert.Equal(MyEnum.Value2, param.ResolveValue(new ContextModel
                {
                    NullableEnumValue = MyEnum.Value2
                }));
                Assert.Null(param.ResolveValue(new ContextModel
                {
                    NullableEnumValue = null
                }));
            }

            public class ContextModel
            {
                [Param]
                public MyEnum EnumValue { get; set; }

                [Param]
                public MyEnum? NullableEnumValue { get; set; }
            }

            public enum MyEnum
            {
                Value1 = 0,
                Value2 = 1,
                Value3 = 3
            }
        }

        public class ObjectNesting
        {
            [Fact]
            public void can_resolve_nested_object_param_values()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextObject));

                var prop1 = parameters.FirstOrDefault(p => p.Name == "Nested.Prop1");
                var prop2 = parameters.FirstOrDefault(p => p.Name == "Nested.Prop2");

                var context = new ContextObject
                {
                    Nested = new NestedContextObject
                    {
                        Prop1 = 10,
                        Prop2 = "Hello World"
                    }
                };

                var prop1Value = prop1.ValueResolver.ResolveValue(prop1, context);
                Assert.NotNull(prop1Value);
                Assert.Equal(typeof(Int32), prop1Value.GetType());
                Assert.Equal(10, prop1Value);

                var prop2Value = prop2.ValueResolver.ResolveValue(prop2, context);
                Assert.NotNull(prop2Value);
                Assert.Equal(typeof(String), prop2Value.GetType());
                Assert.Equal("Hello World", prop2Value);
            }

            [Fact]
            public void can_handle_null_nested_object()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextObject));

                var prop1 = parameters.FirstOrDefault(p => p.Name == "Nested.Prop1");
                var prop2 = parameters.FirstOrDefault(p => p.Name == "Nested.Prop2");

                var context = new ContextObject
                {
                    Nested = null
                };

                object prop1Value = null;

                Assert.DoesNotThrow(() =>
                {
                    prop1Value = prop1.ValueResolver.ResolveValue(prop1, context);
                });
                Assert.Null(prop1Value);

                object prop2Value = null;

                Assert.DoesNotThrow(() =>
                {
                    prop2Value = prop2.ValueResolver.ResolveValue(prop2, context);
                });
                Assert.Null(prop2Value);
            }

            public class ContextObject
            {
                [Reference]
                public NestedContextObject Nested { get; set; }
            }

            public class NestedContextObject
            {
                [Param]
                public int Prop1 { get; set; }

                [Param]
                public string Prop2 { get; set; }
            }
        }

        public class IndirectReferening
        {
            [Fact]
            public void should_return_null_for_invalid_reference_key()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextObject));
                var idParam = parameters.FirstOrDefault(p => p.Name == "RequiredProduct.Id");
                var nameParam = parameters.FirstOrDefault(p => p.Name == "RequiredProduct.Name");

                Assert.NotNull(idParam);
                Assert.NotNull(nameParam);

                var context = new ContextObject
                {
                    RequiredProductId = 5
                };

                var id = nameParam.ValueResolver.ResolveValue(idParam, context);
                var name = nameParam.ValueResolver.ResolveValue(nameParam, context);

                Assert.Null(id);
                Assert.Null(name);
            }

            [Fact]
            public void can_handle_non_nullable_primitive_reference_key()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextObject));

                var productNameParam = parameters.FirstOrDefault(p => p.Name == "RequiredProduct.Name");

                var context = new ContextObject
                {
                    RequiredProductId = 3
                };

                var productName = productNameParam.ValueResolver.ResolveValue(productNameParam, context);
                Assert.Equal("Product 3", productName);
            }

            [Fact]
            public void can_handle_nullable_reference_key()
            {
                var provider = new DefaultRuleParameterProvider();
                var parameters = provider.GetParameters(typeof(ContextObject));

                var productNameParam = parameters.FirstOrDefault(p => p.Name == "Product.Name");

                // has value
                var context = new ContextObject
                {
                    ProductId = 2
                };

                var productName = productNameParam.ValueResolver.ResolveValue(productNameParam, context);
                Assert.Equal("Product 2", productName);

                // no value
                context = new ContextObject
                {
                    ProductId = null
                };

                productName = productNameParam.ValueResolver.ResolveValue(productNameParam, context);
                Assert.Null(productName);
            }

            public class ContextObject
            {
                [Reference(typeof(Product), ReferenceResolver = typeof(FakeProductReferenceResolver))]
                public int? ProductId { get; set; }

                [Reference(typeof(Product), ReferenceResolver = typeof(FakeProductReferenceResolver))]
                public int RequiredProductId { get; set; }
            }

            public class Product
            {
                [Param]
                public int Id { get; set; }
                [Param]
                public string Name { get; set; }
            }

            public class FakeProductReferenceResolver : IReferenceResolver
            {
                private List<Product> _products;

                public FakeProductReferenceResolver()
                {
                    _products = new List<Product>
                    {
                        new Product { Id = 1, Name = "Product 1" },
                        new Product { Id = 2, Name = "Product 2" },
                        new Product { Id = 3, Name = "Product 3" }
                    };
                }

                public object Resolve(Type referencingType, object referenceKey)
                {
                    var id = (int)referenceKey;
                    return _products.Find(p => p.Id == id);
                }
            }
        }
    }
}
