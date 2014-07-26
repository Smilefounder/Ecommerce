using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Shipping;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    [Dependency(typeof(IShippingMethodQuery))]
    public class ShippingMethodQuery : LocalCommerceQuery<ShippingMethod, Kooboo.Commerce.Shipping.ShippingMethod>, IShippingMethodQuery
    {
        private IShippingMethodService _shippingMethodService;

        public ShippingMethodQuery(IShippingMethodService shippingMethodService)
        {
            _shippingMethodService = shippingMethodService;
        }

        public IShippingMethodQuery ById(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }

        public IShippingMethodQuery ByName(string name)
        {
            Query = Query.Where(x => x.Name == name);
            return this;
        }

        protected override IQueryable<Commerce.Shipping.ShippingMethod> CreateQuery()
        {
            return _shippingMethodService.Query();
        }

        protected override IQueryable<Commerce.Shipping.ShippingMethod> OrderByDefault(IQueryable<Commerce.Shipping.ShippingMethod> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
