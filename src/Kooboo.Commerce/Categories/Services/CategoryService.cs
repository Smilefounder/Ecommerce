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

        public void Create(Category category)
        {
            _categoryRepository.Insert(category);
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category, k => new object[] { k.Id });
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }

        #endregion
    }
}