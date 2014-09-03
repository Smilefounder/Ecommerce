using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;

namespace Kooboo.Commerce.Categories
{
    [Dependency(typeof(CategoryService))]
    public class CategoryService
    {
        private IRepository<Category> _categoryRepository;

        public CategoryService(ICommerceDatabase database)
        {
            _categoryRepository = database.Repository<Category>();
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
            _categoryRepository.Update(category);
            Event.Raise(new CategoryUpdated(category));
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
            Event.Raise(new CategoryDeleted(category));
        }
    }
}