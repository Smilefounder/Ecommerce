using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;

namespace Kooboo.Commerce.Multilingual.Tests.Storage
{
    public class CachedTranslationStoreFacts
    {
        [Fact]
        public void should_fetch_from_underlying_store_if_not_cached()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(It.IsAny<CultureInfo>(), It.IsAny<EntityKey[]>()))
                               .Returns(new[] { CreateBrandTranslation(1, zh_CN, "Apple Translated") });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new [] { typeof(Brand) });
            var translations = store.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) });

            Assert.Equal(1, translations.Length);
            Assert.NotNull(translations[0]);
            Assert.Equal("Apple Translated", translations[0].PropertyTranslations[0].TranslatedText);

            underlyingStoreMock.Verify(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) }), Times.Once());

            translations = store.Find(zh_CN, new[] { EntityKey.Create<Brand>(2) });

            underlyingStoreMock.Verify(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) }), Times.Once());
            underlyingStoreMock.Verify(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(2) }), Times.Once());
        }

        [Fact]
        public void should_fetch_from_cache_on_second_find()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(It.IsAny<CultureInfo>(), It.IsAny<EntityKey[]>()))
                               .Returns(new[] { CreateBrandTranslation(1, zh_CN, "Apple Translated") });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new[] { typeof(Brand) });
            var translations = store.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) });

            underlyingStoreMock.Verify(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) }), Times.Once());

            translations = store.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) });

            underlyingStoreMock.Verify(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) }), Times.Once());
        }

        [Fact]
        public void should_refresh_cache_on_update()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");
            var translationsInDb = new [] { CreateBrandTranslation(1, zh_CN, "Translated Brand1") };

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(zh_CN, new[] { EntityKey.Create<Brand>(1) }))
                               .Returns(translationsInDb);

            underlyingStoreMock.Setup(it => it.AddOrUpdate(zh_CN, EntityKey.Create<Brand>(1), It.IsAny<IEnumerable<PropertyTranslation>>()))
                               .Callback(() =>
                               {
                                   translationsInDb[0].PropertyTranslations[0].TranslatedText = "Translated Brand1 (Update)";
                               });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new [] { typeof(Brand) });
 
            var translation = store.Find(zh_CN, EntityKey.Create<Brand>(1));
            Assert.Equal("Translated Brand1", translation.PropertyTranslations[0].TranslatedText);

            store.AddOrUpdate(zh_CN, EntityKey.Create<Brand>(1), new List<PropertyTranslation>
            {
                new PropertyTranslation("Name", "Brand1", "Translated Brand1 (Update)")
            });

            Assert.Equal("Translated Brand1 (Update)", translationsInDb[0].PropertyTranslations[0].TranslatedText);

            translation = store.Find(zh_CN, EntityKey.Create<Brand>(1));
            Assert.Equal("Translated Brand1 (Update)", translation.PropertyTranslations[0].TranslatedText);
        }

        [Fact]
        public void should_cache_by_culture()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");
            var nl_NL = CultureInfo.GetCultureInfo("nl-NL");

            var brandKey = EntityKey.Create<Brand>(15);

            var cnTranslation = CreateBrandTranslation(15, zh_CN, "Apple China");
            var nlTranslation = CreateBrandTranslation(15, nl_NL, "Apple Dutch");

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(zh_CN, new[] { brandKey }))
                               .Returns(new[] { cnTranslation });

            underlyingStoreMock.Setup(it => it.Find(nl_NL, new[] { brandKey }))
                               .Returns(new[] { nlTranslation });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new[] { typeof(Brand) });

            var translation = store.Find(zh_CN, brandKey);
            Assert.Equal("Apple China", translation.PropertyTranslations[0].TranslatedText);

            translation = store.Find(nl_NL, brandKey);
            Assert.Equal("Apple Dutch", translation.PropertyTranslations[0].TranslatedText);
        }

        [Fact]
        public void can_cache_not_translated_items()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");
            var brandKeys = new[] { EntityKey.Create<Brand>(1), EntityKey.Create<Brand>(2) };
            var brandTranslations = new[] { CreateBrandTranslation(1, zh_CN, "Apple China"), null };

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(zh_CN, brandKeys)).Returns(brandTranslations);

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new[] { typeof(Brand) });

            var actualResult = store.Find(zh_CN, brandKeys);
            Assert.NotNull(actualResult[0]);
            Assert.Equal("Apple China", actualResult[0].PropertyTranslations[0].TranslatedText);
            Assert.Null(actualResult[1]);

            actualResult = store.Find(zh_CN, brandKeys);
            Assert.NotNull(actualResult[0]);
            Assert.Equal("Apple China", actualResult[0].PropertyTranslations[0].TranslatedText);
            Assert.Null(actualResult[1]);

            underlyingStoreMock.Verify(it => it.Find(zh_CN, brandKeys), Times.Once());
        }

        [Fact]
        public void should_combine_result_of_cached_and_uncached()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");
            var allTranslations = new[] {
                CreateBrandTranslation(1, zh_CN, "Apple Translated"), 
                CreateBrandTranslation(2, zh_CN, "Dell Translated") ,
                CreateBrandTranslation(3, zh_CN, "Lenovo Translated")
            };

            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(zh_CN, It.IsAny<EntityKey[]>()))
                               .Returns<CultureInfo, EntityKey[]>((culture, keys) =>
                               {
                                   var result = new List<EntityTransaltion>();
                                   foreach (var key in keys)
                                   {
                                       result.Add(allTranslations.First(it => it.EntityKey.Equals(key)));
                                   }

                                   return result.ToArray();
                               });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new[] { typeof(Brand) });

            var translations = store.Find(zh_CN, EntityKey.Create<Brand>(1), EntityKey.Create<Brand>(2));
            Assert.NotNull(translations[0]);
            Assert.NotNull(translations[1]);
            Assert.Equal("Apple Translated", translations[0].PropertyTranslations[0].TranslatedText);
            Assert.Equal("Dell Translated", translations[1].PropertyTranslations[0].TranslatedText);

            translations = store.Find(zh_CN, EntityKey.Create<Brand>(1), EntityKey.Create<Brand>(3));
            Assert.NotNull(translations[0]);
            Assert.NotNull(translations[1]);
            Assert.Equal("Apple Translated", translations[0].PropertyTranslations[0].TranslatedText);
            Assert.Equal("Lenovo Translated", translations[1].PropertyTranslations[0].TranslatedText);
        }

        [Fact]
        public void should_only_cache_types_configured_to_be_cached()
        {
            var zh_CN = CultureInfo.GetCultureInfo("zh-CN");
            var underlyingStoreMock = new Mock<ITranslationStore>();
            underlyingStoreMock.Setup(it => it.Find(zh_CN, new[] { EntityKey.Create<Product>(1) }))
                               .Returns(new[] { 
                                   new EntityTransaltion("zh-CN", EntityKey.Create<Product>(1), new List<PropertyTranslation>
                                   {
                                        new PropertyTranslation("Name", "Product1", "Translated Product1")
                                   })
                               });

            var store = new CachedTranslactionStore(underlyingStoreMock.Object, new[] { typeof(Brand) });

            store.Find(zh_CN, new[] { EntityKey.Create<Product>(1) });
            store.Find(zh_CN, new[] { EntityKey.Create<Product>(1) });

            underlyingStoreMock.Verify(it => it.Find(CultureInfo.GetCultureInfo("zh-CN"), new[] { EntityKey.Create<Product>(1) }), Times.Exactly(2));
        }

        private EntityTransaltion CreateBrandTranslation(int brandId, CultureInfo culture, string translatedName)
        {
            return new EntityTransaltion(culture.Name, EntityKey.Create<Brand>(brandId), new List<PropertyTranslation>
            {
                new PropertyTranslation("Name", "Apple", translatedName)
            });
        }
    }
}
