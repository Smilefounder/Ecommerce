using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Mvc.ModelBinding
{
    /// <summary>
    /// A model binder for System.Object type based on BindingType request parameter.
    /// </summary>
    public class BindingTypeAwareModelBinder : DefaultModelBinder
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
            var key = "BindingType";
            if (!String.IsNullOrEmpty(bindingContext.ModelName))
            {
                key = bindingContext.ModelName + "." + key;
            }

            if (bindingContext.ValueProvider.ContainsPrefix(key))
            {
                modelType = System.Type.GetType(((string[])bindingContext.ValueProvider.GetValue(key).RawValue)[0]);
            }

            return modelType;
        }
    }
}