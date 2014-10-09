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
        private CommerceInstance _instance;
        private IRepository<Category> _categoryRepository;

        public CategoryService(CommerceInstance instance)
        {
            _instance = instance;
            _categoryRepository = instance.Database.Repository<Category>();
        }

        public Category Find(int id)
        {
            return _categoryRepository.Find(id);
        }

        public IQueryable<Category> Query()
        {
            return _categoryRepository.Query();
        }

        public void Create(Category category)
        {
            _categoryRepository.Create(category);
            Event.Raise(new CategoryCreated(category), new EventContext(_instance));
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category);
            Event.Raise(new CategoryUpdated(category), new EventContext(_instance));
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
            Event.Raise(new CategoryDeleted(category), new EventContext(_instance));
        }
    }
}