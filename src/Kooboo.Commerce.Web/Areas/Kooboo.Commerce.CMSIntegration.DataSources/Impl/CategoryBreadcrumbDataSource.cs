using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(CategoryBreadcrumbDataSource))]
    public class CategoryBreadcrumbDataSource : ICommerceDataSource
    {
        public string Name
        {
            get { return "CategoryBreadcrumb"; }
        }

        public string EditorVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/CategoryBreadcrumb.cshtml";
            }
        }

        [DataMember]
        public string CurrentCategoryId { get; set; }

        public object Execute(CommerceDataSourceContext context)
        {
            var categoryId = context.ResolveFieldValue<int>(CurrentCategoryId, 0);
            if (categoryId > 0)
            {
                return context.Site.Commerce().Categories.Breadcrumb(categoryId);
            }

            return null;
        }

        public IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return new Dictionary<string, object>();
        }

        public IEnumerable<string> GetParameters()
        {
            return Enumerable.Empty<string>();
        }

        public bool IsEnumerable()
        {
            return true;
        }
    }
}