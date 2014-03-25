using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    /// <summary>
    /// customer loyalty
    /// loyalty will be used for calculating the value that customer to the site
    /// </summary>
    public class CustomerLoyalty
    {
        /// <summary>
        /// customer id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// saving points
        /// </summary>
        public int SavingPoints { get; set; }
    }
}
