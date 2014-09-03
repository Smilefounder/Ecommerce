using AutoMapper;
using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Brands
{
    public class BrandMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Brand, IdName>();
            Mapper.CreateMap<BrandCustomField, NameValue>();
            Mapper.CreateMap<Brand, BrandEditorModel>();
        }
    }
}