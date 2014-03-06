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
        [GridColumn(GridItemColumnType = typeof (EditGridActionItemColumn))]
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
            Name = String.Format("{0} {1} {2}", customer.FirstName, customer.MiddleName, customer.LastName);
            Email = customer.Email;
            Orders = orders;
        }
    }
}