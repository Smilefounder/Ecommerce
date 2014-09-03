using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Customers
{
    [Grid]
    public class CustomerEditorModel
    {
        public int Id { get; set; }

        public string Group { get; set; }

        public int SavingPoints { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        public Gender Gender { get; set; }

        public IList<AddressModel> Addresses { get; set; }

        public IList<NameValue> CustomFields { get; set; }

        public CustomerEditorModel()
        {
            Addresses = new List<AddressModel>();
            CustomFields = new List<NameValue>();
        }
    }
}