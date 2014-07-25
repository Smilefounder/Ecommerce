using Kooboo.Commerce.API.LocalProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Kooboo.Commerce.Api.Local.Tests
{
    public class IncludeCollectionFacts
    {
        [Fact]
        public void can_do_exact_search()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants");
            includes.Add("Categories");

            Assert.True(includes.Includes("Variants"));
            Assert.True(includes.Includes("Categories"));

            includes = new IncludeCollection();
            includes.Add("Variants.Brand");

            Assert.True(includes.Includes("Variants.Brand"));
        }

        [Fact]
        public void should_do_prefix_search()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants.Brand");

            Assert.True(includes.Includes("Variants"));
            Assert.False(includes.Includes("BVariants"));

            includes = new IncludeCollection();
            includes.Add("Variants.Brand.Supplier");
            Assert.True(includes.Includes("Variants"));
            Assert.True(includes.Includes("Variants.Brand"));
            Assert.True(includes.Includes("Variants.Brand.Supplier"));
        }

        [Fact]
        public void multi_items_with_same_prefix()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants.Brands.Suppliers");
            includes.Add("Variants.Brands.Types");
            includes.Add("Variants.Categories");

            Assert.True(includes.Includes("Variants"));
            Assert.True(includes.Includes("Variants.Brands"));
            Assert.True(includes.Includes("Variants.Categories"));
            Assert.True(includes.Includes("Variants.Brands.Suppliers"));
            Assert.True(includes.Includes("Variants.Brands.Types"));
        }

        [Fact]
        public void prefix_search_should_base_on_dots()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants.Brands.Types");

            Assert.False(includes.Includes("Variants.Bran"));
        }

        [Fact]
        public void can_enumerate_all_paths()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants");
            includes.Add("Categories.Types.Suppliers");
            includes.Add("Variants.Prices.Types");
            includes.Add("Brands.Categories");
            includes.Add("OrderItems");

            var paths = includes.ToList();
            Assert.Equal(4, paths.Count);
            Assert.Contains("Variants.Prices.Types", paths);
            Assert.Contains("Categories.Types.Suppliers", paths);
            Assert.Contains("Brands.Categories", paths);
            Assert.Contains("OrderItems", paths);
        }

        [Fact]
        public void should_discard_duplicates()
        {
            var includes = new IncludeCollection();
            includes.Add("Variants.Prices.Types");
            includes.Add("Variants");
            includes.Add("Variants.Prices");
            includes.Add("Variants.Prices.Types");

            var paths = includes.ToList();
            Assert.Equal(1, paths.Count);
            Assert.Equal("Variants.Prices.Types", paths[0]);
        }
    }
}
