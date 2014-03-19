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

        public IQueryable<Brand> Query()
        {
            return _brandRepository.Query();
        }

        //public IPagedList<Brand> GetAllBrands(int? pageIndex, int? pageSize)
        //{
        //    var query = _brandRepository.Query();
        //    query = query.OrderByDescending(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        public bool Create(Brand brand)
        {
            return _brandRepository.Insert(brand);
        }

        public bool Update(Brand brand)
        {
            return _brandRepository.Update(brand, k => new object[] { k.Id });
        }

        public bool Save(Brand brand)
        {
            if (brand.Id > 0)
            {
                bool exists = _brandRepository.Query(o => o.Id == brand.Id).Any();
                if (exists)
                    return Update(brand);
                else
                    return Create(brand);
            }
            else
            {
                return Create(brand);
            }
        }

        public bool Delete(Brand brand)
        {
            return _brandRepository.Delete(brand);
        }

        #endregion
    }
}