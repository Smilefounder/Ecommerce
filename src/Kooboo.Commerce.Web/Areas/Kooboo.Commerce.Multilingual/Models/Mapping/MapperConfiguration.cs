using AutoMapper;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models.Mapping
{
    static class MapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Product, ProductModel>().ForMember(p => p.Variants, opt => opt.Ignore());
            Mapper.CreateMap<ProductCustomField, CustomFieldModel>();
            Mapper.CreateMap<ProductVariantField, CustomFieldModel>();

            Mapper.CreateMap<ProductType, ProductTypeModel>();
            Mapper.CreateMap<CustomFieldDefinition, CustomFieldDefinitionModel>();
        }
    }
}