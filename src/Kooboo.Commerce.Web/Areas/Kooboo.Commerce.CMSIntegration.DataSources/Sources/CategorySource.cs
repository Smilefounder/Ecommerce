using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class CategorySource : ApiCommerceSource
    {
        public CategorySource()
            : base("Categories", typeof(ICategoryQuery), typeof(Category))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Categories;
        }
    }
}