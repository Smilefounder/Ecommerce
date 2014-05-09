using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class RegisterModel
    {
        public bool ClubCard { get; set; }

        public string ClubCardNumber { get; set; }

        public string ClubCardPostcode { get; set; }

        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Birthday { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string BankNumber { get; set; }

        public string Postcode { get; set; }

        public string HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public int Country { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string WhereKnowUsFrom { get; set; }

        public string FavoriteCategories { get; set; }

        public bool AcceptNewsletter { get; set; }
    }
}
