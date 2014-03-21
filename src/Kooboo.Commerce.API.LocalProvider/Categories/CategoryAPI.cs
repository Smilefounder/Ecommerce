using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.Categories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.LocalProvider.Categories
{
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

        protected override IQueryable<Commerce.Categories.Category> CreateQuery()
        {
            return _categoryService.Query();
        }

        protected override IQueryable<Commerce.Categories.Category> OrderByDefault(IQueryable<Commerce.Categories.Category> query)
        {
            return query.OrderBy(o => o.Id);
        }

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

        public ICategoryQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id).OrderBy(o => o.Name);
            return this;
        }

        public ICategoryQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }

        public ICategoryQuery Published(bool published)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Published == published);
            return this;
        }

        public ICategoryQuery ByParentId(int? parentId)
        {
            EnsureQuery();
            if (parentId.HasValue)
                _query = _query.Where(o => o.Parent.Id == parentId.Value);
            else
                _query = _query.Where(o => o.Parent == null);
            return this;
        }

        public ICategoryQuery LoadWithParent()
        {
            _loadWithParent = true;
            return this;
        }

        public ICategoryQuery LoadWithAllParents()
        {
            _loadWithParents = true;
            return this;
        }

        public ICategoryQuery LoadWithChildren()
        {
            _loadWithChildren = true;
            return this;
        }

        //private void LoadParent(Category category)
        //{
        //    var parent = _categoryService.Query().Where(o => o.Children.Any(c => c.Id == category.Id)).FirstOrDefault();
        //    if (parent != null)
        //        category.Parent = Map(parent);
        //}

        //private void LoadChildren(Category category)
        //{
        //    var children = _categoryService.Query().Where(o => o.Parent.Id == category.Id).ToArray();
        //    if (children != null)
        //        category.Children = children.Select(o => Map(o)).ToArray();
        //}

        //private void LoadWithOptions(Category category)
        //{
        //    if (_loadWithParents)
        //    {
        //        Category that = category;
        //        while (that != null)
        //        {
        //            LoadParent(that);
        //            that = that.Parent;
        //        }
        //    }
        //    else if (_loadWithParent)
        //    {
        //        LoadParent(category);
        //    }
        //    if (_loadWithChildren)
        //    {
        //        LoadChildren(category);
        //    }
        //}
        protected override void OnQueryExecuted()
        {
            _loadWithParent = false;
            _loadWithParents = false;
            _loadWithChildren = false;
        }

        public ICategoryQuery Query()
        {
            return this;
        }
    }
}
