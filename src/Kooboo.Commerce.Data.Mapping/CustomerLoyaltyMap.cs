using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.Data.Mapping
{
    public class CustomerLoyaltyMap : EntityTypeConfiguration<CustomerLoyalty>
    {
        public CustomerLoyaltyMap()
        {
            HasKey(customerLoyalty => customerLoyalty.CustomerId);
        }
    }
}