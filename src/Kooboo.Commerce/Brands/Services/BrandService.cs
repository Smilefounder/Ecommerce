using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Brands.Services
{
    [Dependency(typeof (IBrandService))]
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;

        public BrandService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        #region IBrandService Members

        public Brand GetById(int id)
        {
            return _brandRepository.Get(o => o.Id == id);
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandRepository.Query().ToArray();
        }

        public IPagedList<Brand> GetAllBrands(int? pageIndex, int? pageSize)
        {
            var query = _brandRepository.Query();
            query = query.OrderByDescending(o => o.Id);
            return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        }

        public void Create(Brand brand)
        {
            _brandRepository.Insert(brand);
        }

        public void Update(Brand brand)
        {
            _brandRepository.Update(brand, k => new object[] { k.Id });
        }

        public void Delete(Brand brand)
        {
            _brandRepository.Delete(brand);
        }

        #endregion
    }
}