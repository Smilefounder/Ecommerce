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
        private readonly ICommerceDatabase _database;

        public BrandService(ICommerceDatabase database)
        {
            _database = database;
        }

        public Brand GetById(int id)
        {
            var query = Query();
            return query.Where(o => o.Id == id).FirstOrDefault();
        }

        public IQueryable<Brand> Query()
        {
            return _database.Query<Brand>();
        }

        public void Create(Brand brand)
        {
            _database.Insert(brand);;
            Event.Raise(new BrandCreated(brand));
        }

        public void Update(Brand brand)
        {
            _database.Repository<Brand>().Update(brand);
            Event.Raise(new BrandUpdated(brand));
        }

        public void Delete(Brand brand)
        {
            _database.Repository<Brand>().Delete(brand);
            Event.Raise(new BrandDeleted(brand));
        }
    }
}