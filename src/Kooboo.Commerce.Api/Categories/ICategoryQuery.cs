using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    /// <summary>
    /// category query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface ICategoryQuery : ICommerceQuery<Category>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>category query</returns>
        ICategoryQuery ById(int id);
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">category name</param>
        /// <returns>category query</returns>
        ICategoryQuery ByName(string name);
        /// <summary>
        /// add published filter to query
        /// normally the api doesn't set the published as implicted. you need to explicit add this filter at the front-site.
        /// </summary>
        /// <param name="published">category id</param>
        /// <returns>category query</returns>
        ICategoryQuery Published(bool published);

        /// <summary>
        /// add parent id filter to query
        /// </summary>
        /// <param name="parentId">category parent id</param>
        /// <returns>category query</returns>
        ICategoryQuery ByParentId(int? parentId);
        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>category query</returns>
        ICategoryQuery ByCustomField(string customFieldName, string fieldValue);        
    }
}
