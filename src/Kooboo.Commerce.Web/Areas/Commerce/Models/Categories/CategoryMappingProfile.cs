using AutoMapper;
using Kooboo.Commerce.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Categories
{
    public class CategoryMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Category, IdName>();
            Mapper.CreateMap<CategoryCustomField, NameValue>();
            Mapper.CreateMap<Category, CategoryEditorModel>();
        }
    }
}