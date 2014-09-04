using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Categories;
using Kooboo.Commerce.Api.Metadata;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(CategoryBreadcrumbDataSource))]
    public class CategoryBreadcrumbDataSource : GenericCommerceDataSource
    {
        public override string Name
        {
            get { return "CategoryBreadcrumb"; }
        }

        public override bool SupportTakeOperationSelection
        {
            get
            {
                return false;
            }
        }

        public override IEnumerable<Api.Metadata.FilterDescription> Filters
        {
            get
            {
                yield return new FilterDescription("ByCurrentProduct", new Int32ParameterDescription("CurrentProductId", false));
                yield return new FilterDescription("ByCurrentCategory", new Int32ParameterDescription("CurrentCategoryId", false));
            }
        }

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            int? currentCategoryId = null;

            var productFilter = settings.Filters.Find(f => f.Name == "ByCurrentProduct");
            if (productFilter != null)
            {
                var productId = (int)productFilter.ParameterValues["CurrentProductId"];
                var product = context.Site.Commerce().Products.Query().ById(productId).Include(p => p.Categories).FirstOrDefault();
                if (product != null)
                {
                    var category = product.Categories.FirstOrDefault();
                    if (category != null)
                    {
                        currentCategoryId = category.Id;
                    }
                }
            }

            if (currentCategoryId == null)
            {
                var categoryFilter = settings.Filters.Find(f => f.Name == "ByCurrentCategory");
                if (categoryFilter != null)
                {
                    currentCategoryId = (int)categoryFilter.ParameterValues["CurrentCategoryId"];
                }
            }

            if (currentCategoryId == null)
            {
                return null;
            }

            return context.Site.Commerce().Categories.Breadcrumb(currentCategoryId.Value);
        }
    }
}