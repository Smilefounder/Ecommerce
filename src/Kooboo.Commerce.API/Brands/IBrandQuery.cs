using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Brands.Services
{
    public interface IBrandQuery : ICommerceQuery<Brand>
    {
        IBrandQuery ById(int id);
        IBrandQuery ByName(string name);
    }
}
