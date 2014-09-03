using System.Linq;
using Kooboo.Commerce.Api.Shipping;
using Core = Kooboo.Commerce.Shipping;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    public class ShippingMethodQueryExecutor : QueryExecutorBase<ShippingMethod, Core.ShippingMethod>
    {
        public ShippingMethodQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.ShippingMethod> CreateLocalQuery()
        {
            return ApiContext.Database.GetRepository<Core.ShippingMethod>().Query().OrderBy(it => it.Id);
        }

        protected override IQueryable<Core.ShippingMethod> ApplyFilter(IQueryable<Core.ShippingMethod> query, QueryFilter filter)
        {
            if (filter.Name == ShippingMethodFilters.ById.Name)
            {
                var id = (int)filter.Parameters["Id"];
                query = query.Where(it => it.Id == id);
            }
            else if (filter.Name == ShippingMethodFilters.ByName.Name)
            {
                var name = (string)filter.Parameters["Name"];
                query = query.Where(it => it.Name == name);
            }

            return query;
        }
    }
}
