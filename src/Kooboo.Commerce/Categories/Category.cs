using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Rules;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Categories
{
    public class Category : ILocalizable
    { 
        [Param]
        public int Id { get; set; } 

        [Param, Localizable, StringLength(50)]
        public string Name { get; set; } 

        [Localizable, StringLength(1000)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Photo { get; set; }

        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }

        public virtual ICollection<CategoryCustomField> CustomFields { get; set; }

        public Category()
        {
            Children = new List<Category>();
            CustomFields = new List<CategoryCustomField>();
        }

        public void SetCustomField(string name, string value)
        {
            var field = CustomFields.FirstOrDefault(f => f.Name == name);
            if (field == null)
            {
                field = new CategoryCustomField(name, value);
                CustomFields.Add(field);
            }
            else
            {
                field.Value = value;
            }
        }
    }
}
