using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Categories.Services
{
    public interface ICategoryService
    {
        Category GetById(int id);

        IPagedList<Category> GetRootCategories(int? pageIndex, int? pageSize);

        IEnumerable<Category> GetChildCategories(int parentId);

        void Create(Category category);

        void Update(Category category);

        void Delete(Category category);

        IQueryable<Category> GetRootCategories();
    }
}