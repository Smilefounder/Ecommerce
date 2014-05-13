using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    /// <summary>
    /// customer query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface ICustomerQuery : ICommerceQuery<Customer>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer query</returns>
        ICustomerQuery ById(int id);

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">customer account id</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByAccountId(string accountId);

        /// <summary>
        /// add first name filter to query
        /// </summary>
        /// <param name="firstName">customer first name</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByFirstName(string firstName);

        /// <summary>
        /// add middle name filter to query
        /// </summary>
        /// <param name="middleName">customer middle name</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByMiddleName(string middleName);

        /// <summary>
        /// add last name filter to query
        /// </summary>
        /// <param name="lastName">customer last name</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByLastName(string lastName);

        /// <summary>
        /// add email filter to query
        /// </summary>
        /// <param name="email">customer email</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByEmail(string email);

        /// <summary>
        /// add gender filter to query
        /// </summary>
        /// <param name="gender">customer gender</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByGender(Gender gender);

        /// <summary>
        /// add phone filter to query
        /// </summary>
        /// <param name="phone">customer phone</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByPhone(string phone);

        /// <summary>
        /// add city filter to query
        /// </summary>
        /// <param name="city">customer city</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByCity(string city);

        /// <summary>
        /// add country id filter to query
        /// </summary>
        /// <param name="countryId">customer country id</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByCountry(int countryId);
        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>customer query</returns>
        ICustomerQuery ByCustomField(string customFieldName, string fieldValue);
    }
}
