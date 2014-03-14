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

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }

        public IList<SelectListItem> GenderList { get; set; }

        [Required(ErrorMessage = "Required")]
        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public int? CountryId { get; set; }

        public string City { get; set; }

        public CustomerEditorModel()
        {
        }

        public CustomerEditorModel(Customer customer)
        {
            Id = customer.Id;

            Email = customer.Email;
            FirstName = customer.FirstName;
            MiddleName = customer.MiddleName;
            LastName = customer.LastName;
            Gender = customer.Gender;
            Phone = customer.Phone;
            CountryId = customer.CountryId;
            City = customer.City;
        }

        public void UpdateTo(Customer customer)
        {
            customer.Id = Id;
            customer.Email = Email.Trim();
            customer.FirstName = FirstName;
            customer.MiddleName = MiddleName;
            customer.LastName = LastName;
            customer.Gender = Gender;
            customer.Phone = Phone;
            customer.CountryId = CountryId;
            customer.City = City;
        }
    }
}