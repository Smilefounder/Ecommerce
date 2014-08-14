using Kooboo.Commerce.Api.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.Api.Local.Customers
{
    class CustomerQueryExecutor : QueryExecutorBase<Customer, Core.Customer>
    {
        public CustomerQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.Customer> CreateLocalQuery()
        {
            return ApiContext.Services.Customers.Query().OrderByDescending(c => c.Id);
        }

        protected override IQueryable<Core.Customer> ApplyFilter(IQueryable<Core.Customer> query, QueryFilter filter)
        {
            if (filter.Name == CustomerFilters.ById.Name)
            {
                query = query.Where(c => c.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == CustomerFilters.ByEmail.Name)
            {
                query = query.Where(c => c.Email == (string)filter.Parameters["Email"]);
            }
            else if (filter.Name == CustomerFilters.ByAccountId.Name)
            {
                query = query.Where(c => c.AccountId == (string)filter.Parameters["AccountId"]);
            }
            else if (filter.Name == CustomerFilters.ByCustomField.Name)
            {
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(c => c.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            }

            return query;
        }
    }
}
