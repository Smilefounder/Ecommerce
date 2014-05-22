Entended Query
==============


What's Extended Query
------------------------

User can query data by some filters. However, some filter conditions are often used and some filters are complicate and can’t just simply by setting a value. In this case, we can use `Extended Query`. Extended queries are fully open to developers. By implementing specified extended query interfaces, developer can easily carry out complicated customized queries, such as most valuable customers, top sale products etc. These queries can be complex query which may join other tables with grouping statistics. The extended queries, usually are implemented as an add-in, will be probed automatically and be added to corresponding query pages at the more search section. The admin/user then can click the extended query and list the results.

How to extend a query
---------------------

The common extended query interface is `IExtendedQuery<>`, The `Parameters` property describe your query parameters, which accept user’s input value on the page. And the `Query` method execute your query logic when user click the extended query link. You can use your parameters in the query. Extend an existing query is easy. By steps:

1. Implement a class that derives from `IExtendedQuery<TModel, TQueryModel>`. 
2. Customize your query with your parameters and execute your query logic.
3. Register your class as a service. In kooboo, the easiest way to register the service into ioc container is using the dependency attribute: 
	`[Dependency(typeof(IExtendedQuery<TModel, TQueryModel>), ComponentLifeStyle.Transient, Key = "yourExtendedQueryKey")]`
4. Publish and put the dll file under your commerce site bin directory.


What queries are extendable
---------------------------

In current commerce version, we have three built-in extended queries points. 

1. Customer Extended Query: your extended query must derive from `IExtendedQuery<Customer, CustomerQueryModel>`, and register service as `typeof(IExtendedQuery<Customer, CustomerQueryModel>)`
2. Order Extended Query: your extended query must derive from `IExtendedQuery<Order, OrderQueryModel>`, and register service as `typeof(IExtendedQuery<Order, OrderQueryModel>)`
3. Product Extended Query: your extended query must derive from `IExtendedQuery<Product, Product>`, and register service as `typeof(IExtendedQuery<Product, Product>)`


Sample codes
------------

There are three samples which implement the three **built-in** extended queries under *AddIns/ExtendedQueries* directory of the source codes. Rebuild the solution and start **Kooboo.Commerce.Web** project. 


For example:


    [Dependency(typeof(IExtendedQuery<Customer, CustomerQueryModel>), ComponentLifeStyle.Transient, Key = "RecentOrderedCustomer")] // register as ioc service
    public class RecentOrderedCustomers : IExtendedQuery<Customer, CustomerQueryModel>
    {
        public string Name
        {
            get { return "RecentOrderedCustomer"; }
        }

        public string Title
        {
            get { return "Recent Ordered Customers"; }
        }

        public string Description
        {
            get { return "Customers who placed orders in the last days"; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                        new ExtendedQueryParameter() { Name = "Days", Description = "Customer Ordered Days Before", Type = typeof(System.Int32), DefaultValue = 7 }
                    };
            }
        }

        public IPagedList<TResult> Query<TResult>(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize, Func<CustomerQueryModel, TResult> func)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            int days = 7;
            var para = parameters.FirstOrDefault(o => o.Name == "Days");
            if (para != null && para.Value != null)
                days = Convert.ToInt32(para.Value);
            DateTime lastDate = DateTime.Today.AddDays(-1 * days);

            var orderQuery = db.GetRepository<Order>().Query()
                .Where(o => o.CreatedAtUtc > lastDate);
            IQueryable<CustomerQueryModel> query = db.GetRepository<Customer>().Query()
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .Select(o => new CustomerQueryModel() { Customer = o.Customer, OrdersCount = o.Orders })
                .OrderByDescending(o => o.Customer.Id);
            var total = query.Count();
            var data = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();
            return new PagedList<TResult>(data.Select<CustomerQueryModel, TResult>(o => func(o)), pageIndex, pageSize, total);
        }
    }


You can find the extended queries in the menu items of:

* /Commerce/Customers
* /Commerce/Orders
* /Commerce/Catalog/Products

**Click** the corresponding menu item to enter the list page, and click the downside arrow on the search section at the right of the page. Then you can see your extended queries. **Click** the query name to execute the query, and the parameters link to set your query parameters.


