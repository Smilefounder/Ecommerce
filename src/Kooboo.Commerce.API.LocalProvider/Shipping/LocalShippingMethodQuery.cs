using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.Shipping;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Shipping
{
    [Dependency(typeof(IShippingMethodQuery))]
    public class LocalShippingMethodQuery : LocalCommerceQuery<ShippingMethod, Kooboo.Commerce.Shipping.ShippingMethod>, IShippingMethodQuery
    {
        private IShippingMethodService _shippingMethodService;

        public LocalShippingMethodQuery(
            IHalWrapper halWrapper, 
            IShippingMethodService shippingMethodService,
            IMapper<ShippingMethod, Kooboo.Commerce.Shipping.ShippingMethod> mapper)
            : base(halWrapper, mapper)
        {
            _shippingMethodService = shippingMethodService;
        }

        public IShippingMethodQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(x => x.Id == id);
            return this;
        }

        public IShippingMethodQuery ByName(string name)
        {
            EnsureQuery();
            _query = _query.Where(x => x.Name == name);
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
