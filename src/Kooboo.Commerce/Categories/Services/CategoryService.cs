using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;

namespace Kooboo.Commerce.Categories.Services
{
    [Dependency(typeof(ICategoryService))]
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

        public Category GetById(int id)
        {
            return _categoryRepository.Find(id);
        }

        public IQueryable<Category> Query()
        {
            return _categoryRepository.Query();
        }

        public void Create(Category category)
        {
            _categoryRepository.Insert(category);
            Event.Raise(new CategoryCreated(category));
        }

        public void Update(Category category)
        {
            var dbCategory = _categoryRepository.Find(category.Id);

            dbCategory.CustomFields.Clear();

            if (category.CustomFields != null && category.CustomFields.Count > 0)
            {
                foreach (var field in category.CustomFields)
                {
                    dbCategory.CustomFields.Add(new CategoryCustomField
                    {
                        Name = field.Name,
                        Value = field.Value
                    });
                }
            }

            _categoryRepository.Update(dbCategory, category);

            Event.Raise(new CategoryUpdated(dbCategory));
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
            Event.Raise(new CategoryDeleted(category));
        }
    }
}