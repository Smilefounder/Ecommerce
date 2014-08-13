using Kooboo.Commerce.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class AddressExtensions
    {
        public static Address ById(this IQueryable<Address> query, int addressId)
        {
            return query.FirstOrDefault(x => x.Id == addressId);
        }
    }
}
