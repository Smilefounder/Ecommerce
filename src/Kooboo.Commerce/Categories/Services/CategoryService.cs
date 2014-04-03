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
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<CategoryCustomField> _categoryCustomFieldRepository;

        public CategoryService(ICommerceDatabase db, IRepository<Category> categoryRepository, IRepository<CategoryCustomField> categoryCustomFieldRepository)
        {
            _db = db;
            _categoryRepository = categoryRepository;
            _categoryCustomFieldRepository = categoryCustomFieldRepository;
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
        public IQueryable<CategoryCustomField> CustomFieldsQuery()
        {
            return _categoryCustomFieldRepository.Query();
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
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    _categoryRepository.Update(category, k => new object[] { k.Id });
                    _categoryCustomFieldRepository.DeleteBatch(o => o.CategoryId == category.Id);
                    if (category.CustomFields != null && category.CustomFields.Count > 0)
                    {
                        foreach (var cf in category.CustomFields)
                        {
                            _categoryCustomFieldRepository.Insert(cf);
                        }
                    }
                    tx.Commit();
                }
                return true;
            }
            catch
            {
                return false;
            }
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
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    _categoryCustomFieldRepository.DeleteBatch(o => o.CategoryId == category.Id);
                    _categoryRepository.Delete(category);
                    tx.Commit();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}