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

        void Create(Brand brand);

        void Update(Brand brand);

        void Delete(Brand brand);
    }
}