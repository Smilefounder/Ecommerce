using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.Categories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Categories
{
    public class CategoryQuery : LocalCommerceQuery<Category, Kooboo.Commerce.Categories.Category>, ICategoryQuery
    {
        private ICategoryService _categoryService;
        private bool _loadWithParent = false;
        private bool _loadWithParents = false;
        private bool _loadWithChildren = false;

        public CategoryQuery(ICategoryService categoryService, IMapper<Category, Kooboo.Commerce.Categories.Category> mapper)
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

        public ICategoryQuery ById(int id)
        {
            CreateQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public ICategoryQuery ByName(string name)
        {
            CreateQuery();
            _query = _query.Where(o => o.Name == name);
            return this;
        }

        public ICategoryQuery Published(bool published)
        {
            CreateQuery();
            _query = _query.Where(o => o.Published == published);
            return this;
        }

        public ICategoryQuery ByParentId(int? parentId)
        {
            CreateQuery();
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

        private void LoadParent(Category category)
        {
            category.Parent = _categoryService.Query().Where(o => o.Children.Any(c => c.Id == category.Id)).Select(o => _mapper.MapTo(o)).FirstOrDefault();
        }

        private void LoadChildren(Category category)
        {
            category.Children = _categoryService.Query().Where(o => o.Parent.Id == category.Id).Select(o => _mapper.MapTo(o)).ToArray();
        }

        private void LoadWithOptions(Category category)
        {
            if(_loadWithParents)
            {
                Category that = category;
                while(that != null)
                {
                    LoadParent(that);
                    that = that.Parent;
                }
            }
            else if(_loadWithParent)
            {
                LoadParent(category);
            }
            if(_loadWithChildren)
            {
                LoadChildren(category);
            }
        }

        private void ResetLoadOptions()
        {
            _loadWithParent = false;
            _loadWithParents = false;
            _loadWithChildren = false;
        }

        public override Category[] Pagination(int pageIndex, int pageSize)
        {
            var categories = base.Pagination(pageIndex, pageSize);
            foreach(var cate in categories)
            {
                LoadWithOptions(cate);
            }
            ResetLoadOptions();
            return categories;
        }

        public override Category[] ToArray()
        {
            var categories = base.ToArray();
            foreach (var cate in categories)
            {
                LoadWithOptions(cate);
            }
            ResetLoadOptions();
            return categories;
        }

        public override Category FirstOrDefault()
        {
            var category = base.FirstOrDefault();
            LoadWithOptions(category);
            ResetLoadOptions();
            return category;
        }

        public override void Create(Category obj)
        {
            if (obj != null)
                _categoryService.Create(_mapper.MapFrom(obj));
        }

        public override void Update(Category obj)
        {
            if (obj != null)
                _categoryService.Update(_mapper.MapFrom(obj));
        }

        public override void Delete(Category obj)
        {
            if (obj != null)
                _categoryService.Delete(_mapper.MapFrom(obj));
        }
    }
}
