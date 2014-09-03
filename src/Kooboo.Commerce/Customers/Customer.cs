using System.Collections.Generic;
using System.Linq;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Rules;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Customers
{
    public class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
            CustomFields = new List<CustomerCustomField>();
        }

        [Param]
        public int Id { get; set; }

        [Param, StringLength(50)]
        public string Group { get; set; }

        [Param]
        public int SavingPoints { get; set; }

        [Param, StringLength(50)]
        public string FirstName { get; set; }

        [Param, StringLength(50)]
        public string LastName { get; set; }

        [Param]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Param, StringLength(100)]
        public string Email { get; set; }

        [Param]
        public Gender Gender { get; set; }

        public int? DefaultShippingAddressId { get; set; }

        public int? DefaultBillingAddressId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        [NotMapped]
        public virtual Address DefaultShippingAddress
        {
            get
            {
                if (Addresses != null && DefaultShippingAddressId.HasValue)
                {
                    return Addresses.FirstOrDefault(o => o.Id == DefaultShippingAddressId.Value);
                }
                return null;
            }
        }

        [NotMapped]
        public virtual Address DefaultBillingAddress
        {
            get
            {
                if (Addresses != null && DefaultBillingAddressId.HasValue)
                {
                    return Addresses.FirstOrDefault(o => o.Id == DefaultBillingAddressId.Value);
                }
                return null;
            }
        }

        public virtual ICollection<CustomerCustomField> CustomFields { get; set; }

        public void SetCustomField(string name, string value)
        {
            var field = CustomFields.FirstOrDefault(f => f.Name == name);
            if (field == null)
            {
                field = new CustomerCustomField(name, value);
                CustomFields.Add(field);
            }
            else
            {
                field.Value = value;
            }
        }
    }
}