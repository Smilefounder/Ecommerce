using System.Linq;
using Kooboo.Commerce.Api.Shipping;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    public class ShippingMethodQuery : LocalCommerceQuery<ShippingMethod, Kooboo.Commerce.Shipping.ShippingMethod>, IShippingMethodQuery
    {
        public ShippingMethodQuery(LocalApiContext context)
            : base(context)
        {
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
            return Context.Services.ShippingMethods.Query();
        }

        protected override IQueryable<Commerce.Shipping.ShippingMethod> OrderByDefault(IQueryable<Commerce.Shipping.ShippingMethod> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
