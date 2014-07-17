using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Queries
{
    public class QueryType
    {
        public Type Type { get; private set; }

        public string DisplayName { get; private set; }

        public QueryType(Type type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }


        public static QueryType Of(IQuery query)
        {
            if (query is ICustomerQuery)
            {
                return QueryTypes.Customers;
            }
            if (query is IProductQuery)
            {
                return QueryTypes.Products;
            }
            if (query is IOrderQuery)
            {
                return QueryTypes.Orders;
            }

            throw new NotSupportedException();
        }
    }

    public static class QueryTypes
    {
        public static readonly QueryType Customers = new QueryType(typeof(ICustomerQuery), "Customers");

        public static readonly QueryType Products = new QueryType(typeof(IProductQuery), "Products");

        public static readonly QueryType Orders = new QueryType(typeof(IOrderQuery), "Orders");
    }
}
