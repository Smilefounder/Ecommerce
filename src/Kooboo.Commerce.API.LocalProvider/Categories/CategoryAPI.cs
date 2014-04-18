using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.Categories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.LocalProvider.Categories
{
    /// <summary>
    /// category api
    /// </summary>
    [Dependency(typeof(ICategoryAPI), ComponentLifeStyle.Transient)]
    public class CategoryAPI : LocalCommerceQuery<Category, Kooboo.Commerce.Categories.Category>, ICategoryAPI
    {
        private ICategoryService _categoryService;
        private IMapper<Category, Kooboo.Commerce.Categories.Category> _mapper;
        private bool _loadWithParent = false;
        private bool _loadWithParents = false;
        private bool _loadWithChildren = false;

        public CategoryAPI(ICategoryService categoryService, IMapper<Category, Kooboo.Commerce.Categories.Category> mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Categories.Category> CreateQuery()
        {
            return _categoryService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Categories.Category> OrderByDefault(IQueryable<Commerce.Categories.Category> query)
        {
            return query.OrderBy(o => o.Id);
        }

        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected override Category Map(Commerce.Categories.Category obj)
        {
            List<string> includeComplexPropertyNames = new List<string>();
            if (_loadWithParent || _loadWithParents)
                includeComplexPropertyNames.Add("Parent");
            if (_loadWithParents)
                includeComplexPropertyNames.Add("Parent.Parent");
            if (_loadWithChildren)
                includeComplexPropertyNames.Add("Children");
            return _mapper.MapTo(obj, includeComplexPropertyNames.ToArray());
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>category query</returns>
        public ICategoryQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id).OrderBy(o => o.Name);
            return this;
        }

        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">category name</param>
        /// <returns>category query</returns>
        public ICategoryQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Name == name);
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
            EnsureQuery();
            _query = _query.Where(o => o.Published == published);
            return this;
        }

        /// <summary>
        /// add parent id filter to query
        /// </summary>
        /// <param name="parentId">category parent id</param>
        /// <returns>category query</returns>
        public ICategoryQuery ByParentId(int? parentId)
        {
            EnsureQuery();
            if (parentId.HasValue)
                _query = _query.Where(o => o.Parent.Id == parentId.Value);
            else
                _query = _query.Where(o => o.Parent == null);
            return this;
        }

        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>brand query</returns>
        public ICategoryQuery ByCustomField(string customFieldName, string fieldValue)
        {
            EnsureQuery();
            var customFieldQuery = _categoryService.CustomFieldsQuery().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            _query = _query.Where(o => customFieldQuery.Any(c => c.CategoryId == o.Id));
            return this;
        }

        /// <summary>
        /// load the category/categories with parent
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithParent()
        {
            _loadWithParent = true;
            return this;
        }

        /// <summary>
        /// load the category/categories with all parents
        /// the parents will be null on the top/root level of category
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithAllParents()
        {
            _loadWithParents = true;
            return this;
        }

        /// <summary>
        /// load the category/categories with children
        /// </summary>
        /// <returns>category query</returns>
        public ICategoryQuery LoadWithChildren()
        {
            _loadWithChildren = true;
            return this;
        }
        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected override void OnQueryExecuted()
        {
            base.OnQueryExecuted();
            _loadWithParent = false;
            _loadWithParents = false;
            _loadWithChildren = false;
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
