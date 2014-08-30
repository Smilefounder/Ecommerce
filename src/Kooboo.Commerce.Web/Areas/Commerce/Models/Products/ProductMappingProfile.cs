using AutoMapper;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products
{
    public class ProductMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Product, ProductEditorModel>()
                  .ForMember(it => it.CustomFields, opt =>
                  {
                      opt.MapFrom(s => s.CustomFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
                  })
                  .AfterMap((product, model) =>
                  {
                      var productType = product.ProductType;
                      foreach (var fieldDef in productType.CustomFieldDefinitions)
                      {
                          if (!model.CustomFields.ContainsKey(fieldDef.Name))
                          {
                              model.CustomFields.Add(fieldDef.Name, fieldDef.DefaultValue);
                          }
                      }
                  });

            Mapper.CreateMap<ProductVariant, ProductVariantModel>()
                  .ForMember(it => it.VariantFields, opt =>
                  {
                      opt.MapFrom(s => s.VariantFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
                  })
                  .AfterMap((variant, model) =>
                  {
                      var productType = variant.Product.ProductType;
                      foreach (var fieldDef in productType.VariantFieldDefinitions)
                      {
                          if (!model.VariantFields.ContainsKey(fieldDef.Name))
                          {
                              model.VariantFields.Add(fieldDef.Name, fieldDef.DefaultValue);
                          }
                      }
                  });
        }
    }
}