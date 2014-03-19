using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Brands.Services
{
    public interface IBrandService
    {
        Brand GetById(int id);

        IQueryable<Brand> Query();

        bool Create(Brand brand);

        bool Update(Brand brand);
        bool Save(Brand brand);

        bool Delete(Brand brand);
    }
}