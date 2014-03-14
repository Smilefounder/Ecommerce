using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.API
{
    public interface ICategoryAPI
    {
        IEnumerable<Category> GetAllCategories();

        IEnumerable<Category> GetSubCategories(int parentCategoryId);

        Category GetCategory(int categoryId, bool loadParents = false);
    }
}
