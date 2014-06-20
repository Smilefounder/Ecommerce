using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc.ModelBinding
{
    public class DerivedTypeModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return base.CreateModel(controllerContext, bindingContext, GetModelType(controllerContext, bindingContext, modelType));
        }

        protected override ICustomTypeDescriptor GetTypeDescriptor(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelType = GetModelType(controllerContext, bindingContext, bindingContext.ModelType);
            return new AssociatedMetadataTypeTypeDescriptionProvider(modelType).GetTypeDescriptor(modelType);
        }

        static Type GetModelType(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName + ".BindingType"))
            {
                modelType = System.Type.GetType(((string[])bindingContext.ValueProvider.GetValue
                (bindingContext.ModelName + ".BindingType").RawValue)[0]);
            }

            return modelType;
        }
    }
}