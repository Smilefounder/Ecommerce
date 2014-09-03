using Kooboo.Commerce.Api.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class CustomerQueryExtensions
    {
        public static Query<Customer> ById(this Query<Customer> query, int id)
        {
            query.AddFilter(CustomerFilters.ById.CreateFilter(new { Id = id }));
            return query;
        }

        public static Query<Customer> ByEmail(this Query<Customer> query, string email)
        {
            query.AddFilter(CustomerFilters.ByEmail.CreateFilter(new { Email = email }));
            return query;
        }

        public static Query<Customer> ByCustomField(this Query<Customer> query, string fieldName, string fieldValue)
        {
            query.AddFilter(CustomerFilters.ByCustomField.CreateFilter(new { FieldName = fieldName, FieldValue = fieldValue }));
            return query;
        }
    }
}
