using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Brands;

namespace Kooboo.Commerce.Brands
{
    [Dependency(typeof(BrandService))]
    public class BrandService
    {
        private readonly CommerceInstance _instance;

        public BrandService(CommerceInstance instance)
        {
            _instance = instance;
        }

        public Brand Find(int id)
        {
            var query = Query();
            return query.Where(o => o.Id == id).FirstOrDefault();
        }

        public IQueryable<Brand> Query()
        {
            return _instance.Database.Query<Brand>();
        }

        public void Create(Brand brand)
        {
            _instance.Database.Insert(brand);;
            Event.Raise(new BrandCreated(brand), _instance);
        }

        public void Update(Brand brand)
        {
            _instance.Database.Repository<Brand>().Update(brand);
            Event.Raise(new BrandUpdated(brand), _instance);
        }

        public void Delete(Brand brand)
        {
            _instance.Database.Repository<Brand>().Delete(brand);
            Event.Raise(new BrandDeleted(brand), _instance);
        }
    }
}