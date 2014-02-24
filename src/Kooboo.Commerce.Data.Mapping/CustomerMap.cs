using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.Data.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            //HasRequired(customer => customer.Account)
            //    .WithOptional()
            //    .Map(map => map.MapKey("AccountId"))
            //    .WillCascadeOnDelete();

            HasOptional(customer => customer.Loyalty)
                .WithRequired(customerLoyalty => customerLoyalty.Customer)
                .WillCascadeOnDelete();
        }
    }
}