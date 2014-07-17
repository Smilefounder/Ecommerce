using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI
{
    public static class MvcRoutes
    {
        public static class Products
        {
            public static MvcRoute All()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Product"
                };
            }

            public static MvcRoute List()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Product",
                    Action = "Index"
                };
            }

            public static MvcRoute Edit()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Product",
                    Action = "Edit"
                };
            }
        }

        public static class Customers
        {
            public static MvcRoute All()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Customer"
                };
            }

            public static MvcRoute List()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Customer",
                    Action = "Index"
                };
            }

            public static MvcRoute Edit()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Customer",
                    Action = "Edit"
                };
            }
        }

        public static class Orders
        {
            public static MvcRoute All()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Order"
                };
            }

            public static MvcRoute List()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Order",
                    Action = "Index"
                };
            }

            public static MvcRoute Detail()
            {
                return new MvcRoute
                {
                    Area = "Commerce",
                    Controller = "Order",
                    Action = "Detail"
                };
            }
        }
    }
}
