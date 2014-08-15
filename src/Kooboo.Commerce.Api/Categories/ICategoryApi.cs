using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    public interface ICategoryApi
    {
        Query<Category> Query();

        IList<Category> Breadcrumb(int currentCategoryId);
    }
}
