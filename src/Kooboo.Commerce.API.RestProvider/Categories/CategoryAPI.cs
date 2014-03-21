using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.RestProvider.Categories
{
    [Dependency(typeof(ICategoryAPI), ComponentLifeStyle.Transient)]
    public class CategoryAPI : RestApiQueryBase<Category>, ICategoryAPI
    {
        public ICategoryQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public ICategoryQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        public ICategoryQuery Published(bool published)
        {
            QueryParameters.Add("published", published.ToString());
            return this;
        }

        public ICategoryQuery ByParentId(int? parentId)
        {
            if (parentId.HasValue)
                QueryParameters.Add("parentId", parentId.ToString());
            else
                QueryParameters.Add("parentId", "");
            return this;
        }

        public ICategoryQuery LoadWithParent()
        {
            QueryParameters.Add("LoadWithParent", "true");
            return this;
        }

        public ICategoryQuery LoadWithAllParents()
        {
            QueryParameters.Add("LoadWithAllParents", "true");
            return this;
        }

        public ICategoryQuery LoadWithChildren()
        {
            QueryParameters.Add("LoadWithChildren", "true");
            return this;
        }

        public ICategoryQuery Query()
        {
            return this;
        }
    }
}
