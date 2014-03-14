using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Brands.Services
{
    public interface IBrandService
    {
        Brand GetById(int id);

        IEnumerable<Brand> GetAllBrands();

        IPagedList<Brand> GetAllBrands(int? pageIndex, int? pageSize);

        void Create(Brand brand);

        void Update(Brand brand);

        void Delete(Brand brand);
    }
}