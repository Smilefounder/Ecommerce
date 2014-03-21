using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Categories
{
    public interface ICategoryAPI : ICategoryQuery
    {
        ICategoryQuery Query();
    }
}
