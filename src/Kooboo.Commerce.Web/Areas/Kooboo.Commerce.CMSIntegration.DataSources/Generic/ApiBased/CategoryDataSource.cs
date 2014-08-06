using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class CategoryDataSource : ApiBasedDataSource
    {
        public CategoryDataSource()
            : base("Categories", typeof(ICategoryQuery), typeof(Category))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Categories;
        }
    }
}