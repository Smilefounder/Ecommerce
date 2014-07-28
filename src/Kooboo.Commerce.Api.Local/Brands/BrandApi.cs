using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Brands.Services;
using System.Globalization;

namespace Kooboo.Commerce.Api.Local.Brands
{
    public class BrandApi : LocalCommerceQuery<Brand, Kooboo.Commerce.Brands.Brand>, IBrandApi
    {
        public BrandApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Commerce.Brands.Brand> CreateQuery()
        {
            return Context.Services.Brands.Query();
        }

        protected override IQueryable<Commerce.Brands.Brand> OrderByDefault(IQueryable<Commerce.Brands.Brand> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public IBrandQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public IBrandQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }

        public IBrandQuery ByCustomField(string fieldName, string fieldValue)
        {
            Query = Query.Where(b => b.CustomFields.Any(x => x.Name == fieldName && x.Value == fieldValue));
            return this;
        }
    }
}
