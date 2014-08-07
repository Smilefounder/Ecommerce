using Kooboo.CMS.Common.Runtime.Dependency;
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
    public class CategoryDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "Categories"; }
        }

        protected override Type QueryType
        {
            get { return typeof(ICategoryQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(Category); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Categories;
        }
    }
}