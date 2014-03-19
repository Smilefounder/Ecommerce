using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    public interface ICustomerQuery : ICommerceQuery<Customer>, ICommerceAccess<Customer>
    {
        ICustomerQuery ById(int id);

        ICustomerQuery ByAccountId(string accountId);

        ICustomerQuery ByFirstName(string firstName);

        ICustomerQuery ByMiddleName(string middleName);

        ICustomerQuery ByLastName(string lastName);

        ICustomerQuery ByEmail(string email);

        ICustomerQuery ByGender(Gender gender);

        ICustomerQuery ByPhone(string phone);

        ICustomerQuery ByCity(string city);

        ICustomerQuery ByCountry(int countryId);

        ICustomerQuery LoadWithCountry();
        ICustomerQuery LoadWithAddresses();
        ICustomerQuery LoadWithCustomerLoyalty();
    }
}
