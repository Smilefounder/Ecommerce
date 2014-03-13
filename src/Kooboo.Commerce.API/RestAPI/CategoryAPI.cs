using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Categories.Services;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(ICategoryAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class CategoryAPI : RestApiBase, ICategoryAPI
    {
        public IEnumerable<Category> GetAllCategories()
        {
            return Get<List<Category>>(null);
        }

        public IEnumerable<Category> GetSubCategories(int parentCategoryId)
        {
            QueryParameters.Add("id", parentCategoryId.ToString());
            return Get<List<Category>>("Children");
        }

        public Category GetCategory(int categoryId, bool loadParents = false)
        {
            if (loadParents)
            {
                QueryParameters.Add("id", categoryId.ToString());
                return Get<Category>("Parents");
            }
            else
            {
                return Get<Category>(categoryId.ToString());
            }
        }

        protected override string ApiControllerPath
        {
            get { return "Category"; }
        }
    }
}
