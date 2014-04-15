using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Customers
{
    [Grid(IdProperty = "Id", Checkable = true)]
    public class CustomerRowModel
    {
        [GridColumn(GridItemColumnType = typeof (LinkedGridItemColumn))]
        public int Id { get; set; }

        [GridColumn]
        public string Name { get; set; }

        [GridColumn]
        public string Email { get; set; }

        [GridColumn]
        public int Orders { get; set; }

        public CustomerRowModel()
        {
        }

        public CustomerRowModel(Customer customer, int orders)
        {
            Id = customer.Id;
            Name = customer.FullName;
            if (string.IsNullOrEmpty(Name.Trim()))
                Name = customer.AccountId;
            Email = customer.Email;
            Orders = orders;
        }
    }
}