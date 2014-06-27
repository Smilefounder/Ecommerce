using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories.Services
{
    public interface ICategoryService
    {
        Category GetById(int id);

        IQueryable<Category> Query();

        IQueryable<CategoryCustomField> CustomFields();

        void Create(Category category);

        void Update(Category category);

        void Delete(Category category);
    }
}