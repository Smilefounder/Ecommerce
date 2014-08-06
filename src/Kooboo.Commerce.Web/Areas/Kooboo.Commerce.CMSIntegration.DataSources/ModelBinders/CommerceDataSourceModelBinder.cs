using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.ModelBinders
{
    public class CommerceDataSourceModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var dataSourceTypeName = controllerContext.Controller.ValueProvider.GetValue("CommerceDataSourceType").AttemptedValue;
            var dataSource = EngineContext.Current.Resolve<ICommerceDataSource>(dataSourceTypeName);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => dataSource, dataSource.GetType());

            return dataSource;
        }
    }
}