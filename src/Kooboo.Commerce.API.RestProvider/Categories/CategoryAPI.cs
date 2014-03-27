using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.RestProvider.Categories
{
    /// <summary>
    /// category api
    /// </summary>
    [Dependency(typeof(ICategoryAPI), ComponentLifeStyle.Transient)]
    public class CategoryAPI : RestApiQueryBase<Category>, ICategoryAPI
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>category query</returns>
        public ICategoryQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">category name</param>
        /// <returns>category query</returns>
        public ICategoryQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        /// <summary>
        /// add published filter to query
        /// normally the api doesn't set the published as implicted. you need to explicit add this filter at the front-site.
        /// </summary>
        /// <param name="published">category id</param>
        /// <returns>category query</returns>
        public ICategoryQuery Published(bool published)
        {
            QueryParameters.Add("published", published.ToString());
            return this;
        }

        /// <summary>
        /// add parent id filter to query
        /// </summary>
        /// <param name="parentId">category parent id</param>
        /// <returns>category query</returns>
        public ICategoryQuery ByParentId(int? parentId)
        {
            if (parentId.HasValue)
                QueryParameters.Add("parentId", parentId.ToString());
            else
                QueryParameters.Add("parentId", "");
            return this;
        }

        /// <summary>
        /// load the category/categories with parent
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithParent()
        {
            QueryParameters.Add("LoadWithParent", "true");
            return this;
        }

        /// <summary>
        /// load the category/categories with all parents
        /// the parents will be null on the top/root level of category
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithAllParents()
        {
            QueryParameters.Add("LoadWithAllParents", "true");
            return this;
        }

        /// <summary>
        /// load the category/categories with children
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithChildren()
        {
            QueryParameters.Add("LoadWithChildren", "true");
            return this;
        }

        /// <summary>
        /// create category query 
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery Query()
        {
            return this;
        }
    }
}
