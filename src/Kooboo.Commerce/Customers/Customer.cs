using System.Collections.Generic;
using System.Linq;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Rules;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Param]
        public string Group { get; set; }

        [Param]
        public int SavingPoints { get; set; }

        [Param]
        public string FirstName { get; set; }

        [Param]
        public string LastName { get; set; }

        [Param]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Param]
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

        public void SetCustomFields(IDictionary<string, string> fields)
        {
            foreach (var field in CustomFields.ToList())
            {
                if (!fields.ContainsKey(field.Name))
                {
                    CustomFields.Remove(field);
                }
            }

            foreach (var kv in fields)
            {
                var field = CustomFields.FirstOrDefault(f => f.Name == kv.Key);
                if (field == null)
                {
                    field = new CustomerCustomField
                    {
                        Name = kv.Key,
                        Value = kv.Value
                    };
                    CustomFields.Add(field);
                }
                else
                {
                    field.Value = kv.Value;
                }
            }
        }
    }
}