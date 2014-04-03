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
        IQueryable<CategoryCustomField> CustomFieldsQuery();

        //IEnumerable<Category> GetRootCategories();

        //IPagedList<Category> GetRootCategories(int? pageIndex, int? pageSize);

        //IEnumerable<Category> GetChildCategories(int parentId);

        bool Create(Category category);

        bool Update(Category category);
        bool Save(Category category);

        bool Delete(Category category);
    }
}