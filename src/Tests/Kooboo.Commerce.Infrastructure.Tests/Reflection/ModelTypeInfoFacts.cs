using Kooboo.Commerce.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Reflection
{
    public class ModelTypeInfoFacts
    {
        [Fact]
        public void should_recognize_both_normal_and_collection_properties()
        {
            var prop = new ModelTypeInfo(typeof(MyClass).GetProperty("Name").PropertyType);
            Assert.False(prop.IsCollection);
            Assert.Null(prop.ElementType);
            Assert.False(prop.IsDictionary);
            Assert.Null(prop.DictionaryKeyType);
            Assert.Null(prop.DictionaryValueType);

            prop = new ModelTypeInfo(typeof(MyClass).GetProperty("Prices").PropertyType);
            Assert.True(prop.IsCollection);
            Assert.Equal(typeof(decimal), prop.ElementType);
            Assert.False(prop.IsDictionary);
            Assert.Null(prop.DictionaryKeyType);
            Assert.Null(prop.DictionaryValueType);

            prop = new ModelTypeInfo(typeof(MyClass).GetProperty("Rates").PropertyType);
            Assert.True(prop.IsCollection);
            Assert.Equal(typeof(int), prop.ElementType);
            Assert.False(prop.IsDictionary);
            Assert.Null(prop.DictionaryKeyType);
            Assert.Null(prop.DictionaryValueType);

            prop = new ModelTypeInfo(typeof(MyClass).GetProperty("Categories").PropertyType);
            Assert.True(prop.IsCollection);
            Assert.Equal(typeof(String), prop.ElementType);
            Assert.False(prop.IsDictionary);
            Assert.Null(prop.DictionaryKeyType);
            Assert.Null(prop.DictionaryValueType);

            prop = new ModelTypeInfo(typeof(MyClass).GetProperty("PricesByType").PropertyType);
            Assert.True(prop.IsCollection);
            Assert.Equal(typeof(KeyValuePair<string, decimal>), prop.ElementType);
            Assert.True(prop.IsDictionary);
            Assert.Equal(typeof(string), prop.DictionaryKeyType);
            Assert.Equal(typeof(decimal), prop.DictionaryValueType);
        }

        public class MyClass
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string[] Categories { get; set; }

            public IEnumerable<int> Rates { get; set; }

            public IList<decimal> Prices { get; set; }

            public IDictionary<string, decimal> PricesByType { get; set; }
        }
    }
}
