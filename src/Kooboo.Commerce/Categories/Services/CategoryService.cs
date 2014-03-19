using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Categories.Services
{
    [Dependency(typeof (ICategoryService))]
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region ICategoryService Members

        public Category GetById(int id)
        {
            return _categoryRepository.Get(o => o.Id == id);
        }

        public IQueryable<Category> Query()
        {
            return _categoryRepository.Query();
        }

        //public IEnumerable<Category> GetRootCategories()
        //{
        //    return _categoryRepository.Query(o => o.Parent == null).ToArray();
        //}

        //public IPagedList<Category> GetRootCategories(int? pageIndex, int? pageSize)
        //{
        //    var query = _categoryRepository.Query(o => o.Parent == null);
        //    query = query.OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IEnumerable<Category> GetChildCategories(int parentId)
        //{
        //    var query = _categoryRepository.Query(o => o.Parent.Id == parentId);
        //    return query.ToArray();
        //}

        public bool Create(Category category)
        {
            return _categoryRepository.Insert(category);
        }

        public bool Update(Category category)
        {
            return _categoryRepository.Update(category, k => new object[] { k.Id });
        }

        public bool Save(Category category)
        {
            if (category.Id > 0)
            {
                bool exists = _categoryRepository.Query(o => o.Id == category.Id).Any();
                if (exists)
                    return Update(category);
                else
                    return Create(category);
            }
            else
            {
                return Create(category);
            }
        }

        public bool Delete(Category category)
        {
            return _categoryRepository.Delete(category);
        }

        #endregion
    }
}