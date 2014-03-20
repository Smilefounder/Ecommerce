using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Categories
{
    public interface ICategoryQuery : ICommerceQuery<Category>
    {
        ICategoryQuery ById(int id);
        ICategoryQuery ByName(string name);
        ICategoryQuery Published(bool published);

        ICategoryQuery ByParentId(int? parentId);

        ICategoryQuery LoadWithParent();

        ICategoryQuery LoadWithAllParents();
        ICategoryQuery LoadWithChildren();
    }
}
