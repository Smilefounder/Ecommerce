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
                var id = filter.GetParameterValueOrDefault<int>("Id");
                query = query.Where(c => c.Id == id);
            }
            else if (filter.Name == CustomerFilters.ByEmail.Name)
            {
                var email = filter.GetParameterValueOrDefault<string>("Email");
                query = query.Where(c => c.Email == email);
            }
            else if (filter.Name == CustomerFilters.ByAccountId.Name)
            {
                var accountId = filter.GetParameterValueOrDefault<string>("AccountId");
                query = query.Where(c => c.AccountId == accountId);
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
