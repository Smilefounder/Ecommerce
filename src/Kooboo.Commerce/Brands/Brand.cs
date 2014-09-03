using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Rules;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Brands
{
    public class Brand : ILocalizable
    {
        [Param]
        public int Id { get; set; }

        [Param, Localizable, StringLength(50)]
        public string Name { get; set; }

        [Localizable, StringLength(1000)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Logo { get; set; }

        public virtual ICollection<BrandCustomField> CustomFields { get; set; }

        public Brand()
        {
            CustomFields = new List<BrandCustomField>();
        }

        public void SetCustomField(string name, string value)
        {
            var field = CustomFields.FirstOrDefault(f => f.Name == name);
            if (field == null)
            {
                field = new BrandCustomField(name, value);
                CustomFields.Add(field);
            }
            else
            {
                field.Value = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}