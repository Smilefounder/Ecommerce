using AutoMapper;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes.Mapping
{
    public class ModelMaps : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ProductType, ProductTypeModel>()
                  .BeforeMap((source, model) =>
                  {
                      var service = EngineContext.Current.Resolve<PredefinedCustomFieldService>();
                      model.PredefinedFields = service.Query()
                                                      .OrderBy(f => f.Sequence)
                                                      .ThenBy(f => f.Id)
                                                      .ToList();
                  });
        }
    }
}