using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(CategoryDataSource))]
    public class CategoryDataSource : ApiBasedDataSource<Category>
    {
        public override string Name
        {
            get { return "Categories"; }
        }

        protected override Query<Category> Query(ICommerceApi api)
        {
            return api.Categories.Query();
        }
    }
}