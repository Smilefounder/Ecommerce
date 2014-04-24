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
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<BrandCustomField> _brandCustomFieldRepository;

        public BrandService(ICommerceDatabase db, IRepository<Brand> brandRepository, IRepository<BrandCustomField> brandCustomFieldRepository)
        {
            _db = db;
            _brandRepository = brandRepository;
            _brandCustomFieldRepository = brandCustomFieldRepository;
        }

        #region IBrandService Members

        public Brand GetById(int id)
        {
            var query = Query();
            return query.Where(o => o.Id == id).FirstOrDefault();
        }

        public IQueryable<Brand> Query()
        {
            var query = _brandRepository.Query();
            query = query.Include(o => o.CustomFields);
            return query;
        }

        public IQueryable<BrandCustomField> CustomFieldsQuery()
        {
            return _brandCustomFieldRepository.Query();
        }

        public bool Create(Brand brand)
        {
            return _brandRepository.Insert(brand);
        }

        public bool Update(Brand brand)
        {
            try
            {
                //using (var tx = _db.BeginTransaction())
                {
                    _brandRepository.Update(brand, k => new object[] { k.Id });
                    _brandCustomFieldRepository.DeleteBatch(o => o.BrandId == brand.Id);
                    if (brand.CustomFields != null && brand.CustomFields.Count > 0)
                    {
                        foreach (var cf in brand.CustomFields)
                        {
                            _brandCustomFieldRepository.Insert(cf);
                        }
                    }
                    //tx.Commit();
                }
                return true;
            }
            catch
            {
                return false;
            }
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
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    _brandCustomFieldRepository.DeleteBatch(o => o.BrandId == brand.Id);
                    _brandRepository.Delete(brand);
                    tx.Commit();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}