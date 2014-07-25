using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Kooboo.Commerce.Api.Local.Tests.Mapping
{
    public class DefaultObjectMapperFacts
    {
        public class SimpleMapping
        {
            [Fact]
            public void work_on_simple_properties()
            {
                var mapper = new DefaultObjectMapper();
                var source = new SourceProduct
                {
                    Id = 5,
                    Name = "Product name",
                    Price = 150m,
                    BrandId = 10,
                    Type = SourceProductType.Type2
                };

                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, new MappingContext()) as TargetProduct;

                Assert.Equal(5, target.Id);
                Assert.Equal("Product name", target.Name);
                Assert.Equal(150m, target.Price);
                Assert.Equal(TargetProductType.Type2, target.Type);
            }

            public class SourceProduct
            {
                public int Id { get; set; }

                public string Name { get; set; }

                public decimal Price { get; set; }

                public int? BrandId { get; set; }

                public SourceProductType Type { get; set; }
            }

            public enum SourceProductType
            {
                Type1 = 0,
                Type2 = 1
            }

            public class TargetProduct
            {
                public int Id { get; set; }

                public string Name { get; set; }

                public decimal Price { get; set; }

                public int? BrandId { get; set; }

                public string BrandName { get; set; }

                public TargetProductType Type { get; set; }
            }

            public enum TargetProductType
            {
                Type1 = 0,
                Type2 = 1
            }
        }

        public class ComplexObjectNesting
        {
            [Fact]
            public void can_map_one_level_nested_object()
            {
                var source = new SourceProduct
                {
                    Id = 5,
                    Name = "Product name",
                    Brand = new SourceBrand { Id = 10, Name = "Nike" }
                };

                var mapper = new DefaultObjectMapper();
                var includes = new IncludeCollection();
                includes.Add("Brand");
                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, new MappingContext(includes)) as TargetProduct;

                Assert.NotNull(target.Brand);
                Assert.Equal(10, target.Brand.Id);
                Assert.Equal("Nike", target.Brand.Name);
            }

            [Fact]
            public void can_handle_null_nested_object()
            {
                var source = new SourceProduct
                {
                    Id = 1024,
                    Name = "1024 Product"
                };

                var mapper = new DefaultObjectMapper();
                var includes = new IncludeCollection();
                includes.Add("Brand");

                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, new MappingContext(includes)) as TargetProduct;

                Assert.Equal(1024, target.Id);
                Assert.Equal("1024 Product", target.Name);
                Assert.Null(target.Brand);
            }

            [Fact]
            public void should_ignore_nested_objects_bydefault()
            {
                var source = new SourceProduct
                {
                    Id = 5,
                    Name = "Product 1",
                    Brand = new SourceBrand { Id = 10, Name = "Nike" }
                };

                var mapper = new DefaultObjectMapper();
                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, new MappingContext()) as TargetProduct;

                Assert.Equal("Product 1", target.Name);
                Assert.Null(target.Brand);
            }

            [Fact]
            public void can_map_multi_level_nested_objects()
            {
                var source = new SourceOrderItem
                {
                    Id = 1,
                    Product = new SourceProduct
                    {
                        Id = 11,
                        Name = "Product 1",
                        Brand = new SourceBrand { Id = 21, Name = "Brand 1" }
                    }
                };

                var mapper = new DefaultObjectMapper();
                var includes = new IncludeCollection();
                includes.Add("Product.Brand");

                var target = mapper.Map(source, new TargetOrderItem(), typeof(SourceOrderItem), typeof(TargetOrderItem), null, new MappingContext(includes)) as TargetOrderItem;

                Assert.Equal(1, target.Id);
                Assert.NotNull(target.Product);
                Assert.Equal(11, target.Product.Id);
                Assert.NotNull(target.Product.Brand);
                Assert.Equal(21, target.Product.Brand.Id);
                Assert.Equal("Brand 1", target.Product.Brand.Name);
            }

            public class SourceBrand
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class SourceProduct
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public SourceBrand Brand { get; set; }
            }

            public class TargetBrand
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class TargetProduct
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public TargetBrand Brand { get; set; }
            }

            public class SourceOrderItem
            {
                public int Id { get; set; }

                public SourceProduct Product { get; set; }
            }

            public class TargetOrderItem
            {
                public int Id { get; set; }

                public TargetProduct Product { get; set; }
            }
        }

        public class CollectionMapping
        {
            [Fact]
            public void can_map_collections()
            {
                var source = new SourceProduct
                {
                    Id = 10,
                    Variants = new List<SourceProductVariant>
                    {
                        new SourceProductVariant { Id = 1, Name = "variant 1"},
                        new SourceProductVariant { Id = 2, Name = "variant 2"}
                    }
                };

                var mapper = new DefaultObjectMapper();
                var context = new MappingContext();
                context.Includes.Add("Variants");

                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, context) as TargetProduct;

                Assert.Equal(10, target.Id);
                Assert.NotNull(target.Variants);
                Assert.Equal(2, target.Variants.Count);

                Assert.Equal(1, target.Variants.First().Id);
                Assert.Equal("variant 1", target.Variants.First().Name);
                Assert.Equal(2, target.Variants.Skip(1).First().Id);
                Assert.Equal("variant 2", target.Variants.Skip(1).First().Name);
            }

            [Fact]
            public void can_work_on_different_collection_type_array()
            {
                var source = new SourceProduct
                {
                    Id = 10,
                    Variants = new List<SourceProductVariant>
                    {
                        new SourceProductVariant { Id = 1, Name = "variant 1"},
                        new SourceProductVariant { Id = 2, Name = "variant 2"}
                    }
                };

                var mapper = new DefaultObjectMapper();
                var context = new MappingContext();
                context.Includes.Add("Variants");

                var target = mapper.Map(source, new TargetProduct_ArrayVariants(), typeof(SourceProduct), typeof(TargetProduct_ArrayVariants), null, context) as TargetProduct_ArrayVariants;

                Assert.Equal(10, target.Id);
                Assert.NotNull(target.Variants);
                Assert.Equal(2, target.Variants.Length);

                Assert.Equal(1, target.Variants.First().Id);
                Assert.Equal("variant 1", target.Variants.First().Name);
                Assert.Equal(2, target.Variants.Skip(1).First().Id);
                Assert.Equal("variant 2", target.Variants.Skip(1).First().Name);
            }

            [Fact]
            public void can_work_on_different_collection_type_list()
            {
                var source = new SourceProduct
                {
                    Id = 10,
                    Variants = new List<SourceProductVariant>
                    {
                        new SourceProductVariant { Id = 1, Name = "variant 1"},
                        new SourceProductVariant { Id = 2, Name = "variant 2"}
                    }
                };

                var mapper = new DefaultObjectMapper();
                var context = new MappingContext();
                context.Includes.Add("Variants");

                var target = mapper.Map(source, new TargetProduct_IListVariants(), typeof(SourceProduct), typeof(TargetProduct_IListVariants), null, context) as TargetProduct_IListVariants;

                Assert.Equal(10, target.Id);
                Assert.NotNull(target.Variants);
                Assert.Equal(2, target.Variants.Count);

                Assert.Equal(1, target.Variants.First().Id);
                Assert.Equal("variant 1", target.Variants.First().Name);
                Assert.Equal(2, target.Variants.Skip(1).First().Id);
                Assert.Equal("variant 2", target.Variants.Skip(1).First().Name);
            }

            [Fact]
            public void ignore_collections_bydefault()
            {
                var source = new SourceProduct
                {
                    Id = 10,
                    Variants = new List<SourceProductVariant>
                    {
                        new SourceProductVariant { Id = 1, Name = "variant 1"},
                        new SourceProductVariant { Id = 2, Name = "variant 2"}
                    }
                };

                var mapper = new DefaultObjectMapper();
                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, new MappingContext()) as TargetProduct;

                Assert.Equal(10, target.Id);
                Assert.Null(target.Variants);
            }

            [Fact]
            public void should_replace_target_collections()
            {
                var source = new SourceProduct
                {
                    Id = 10,
                    Variants = new List<SourceProductVariant>
                    {
                        new SourceProductVariant { Id = 1, Name = "variant 1"},
                        new SourceProductVariant { Id = 2, Name = "variant 2"}
                    }
                };

                var mapper = new DefaultObjectMapper();
                var context = new MappingContext();
                context.Includes.Add("Variants");

                var target = new TargetProduct();
                target.Variants = new List<TargetProductVariant>();
                target.Variants.Add(new TargetProductVariant { Id = 3, Name = "variant 3" });

                target = mapper.Map(source, target, typeof(SourceProduct), typeof(TargetProduct), null, context) as TargetProduct;

                Assert.Equal(2, target.Variants.Count);
                Assert.Equal(1, target.Variants.First().Id);
                Assert.Equal("variant 1", target.Variants.First().Name);
                Assert.Equal(2, target.Variants.Skip(1).First().Id);
                Assert.Equal("variant 2", target.Variants.Skip(1).First().Name);
            }

            public class SourceProduct
            {
                public int Id { get; set; }
                public ICollection<SourceProductVariant> Variants { get; set; }
            }

            public class SourceProductVariant
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class TargetProduct
            {
                public int Id { get; set; }
                public ICollection<TargetProductVariant> Variants { get; set; }
            }
            public class TargetProduct_ArrayVariants
            {
                public int Id { get; set; }
                public TargetProductVariant[] Variants { get; set; }
            }
            public class TargetProduct_IListVariants
            {
                public int Id { get; set; }
                public IList<TargetProductVariant> Variants { get; set; }
            }
            public class TargetProductVariant
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class CustomizedMappers
        {
            [Fact]
            public void can_customize_nested_object_mapping()
            {
                var source = new SourceProduct
                {
                    Id = 1,
                    Brand = new SourceBrand
                    {
                        Name = "Nike"
                    }
                };

                var mapper = new DefaultObjectMapper();
                var context = new MappingContext();
                context.Includes.Add("Brand");

                mapper.GetMapper = (sourceType, targetType) =>
                {
                    if (sourceType == typeof(SourceBrand))
                    {
                        return new BrandMapper();
                    }

                    return mapper;
                };

                var target = mapper.Map(source, new TargetProduct(), typeof(SourceProduct), typeof(TargetProduct), null, context) as TargetProduct;

                Assert.Equal("Nike (Customized)", target.Brand.Name);
            }

            [Fact]
            public void can_customize_collection_element_mapping()
            {
                var source = new SourceBrandCollection
                {
                    Brands = new List<SourceBrand> {
                        new SourceBrand { Name = "Nike" },
                        new SourceBrand { Name = "Puma" }
                    }
                };

                var mapper = new DefaultObjectMapper();
                mapper.GetMapper = (sourceType, targetType) =>
                {
                    if (sourceType == typeof(SourceBrand))
                    {
                        return new BrandMapper();
                    }

                    return mapper;
                };

                var context = new MappingContext();
                context.Includes.Add("Brands");

                var target = mapper.Map(source, new TargetBrandCollection(), typeof(SourceBrandCollection), typeof(TargetBrandCollection), null, context) as TargetBrandCollection;

                Assert.Equal(2, target.Brands.Count);
                Assert.Equal("Nike (Customized)", target.Brands.First().Name);
                Assert.Equal("Puma (Customized)", target.Brands.Skip(1).First().Name);
            }

            public class SourceProduct
            {
                public int Id { get; set; }
                public SourceBrand Brand { get; set; }
            }
            public class SourceBrand
            {
                public string Name { get; set; }
            }

            public class TargetProduct
            {
                public int Id { get; set; }
                public TargetBrand Brand { get; set; }
            }
            public class TargetBrand
            {
                public string Name { get; set; }
            }

            public class BrandMapper : DefaultObjectMapper
            {
                public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
                {
                    var brand = base.Map(source, target, sourceType, targetType, prefix, context) as TargetBrand;
                    brand.Name += " (Customized)";
                    return brand;
                }
            }

            public class SourceBrandCollection
            {
                public ICollection<SourceBrand> Brands { get; set; }
            }
            public class TargetBrandCollection
            {
                public ICollection<TargetBrand> Brands { get; set; }
            }
        }
    }
}
