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
            Mapper.CreateMap<ProductType, ProductTypeModel>();
            Mapper.CreateMap<CustomFieldDefinition, CustomFieldDefinitionModel>();
        }
    }
}