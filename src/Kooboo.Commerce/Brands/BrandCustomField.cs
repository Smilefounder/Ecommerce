using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Brands
{
    public class BrandCustomField : IOrphanable
    {
        [Key]
        protected int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; protected set; }

        public string Value { get; set; }

        [Column]
        protected int? BrandId { get; set; }

        protected BrandCustomField() { }

        public BrandCustomField(string name, string value)
        {
            Require.NotNullOrEmpty(name, "name");
            Name = name;
            Value = value;
        }

        bool IOrphanable.IsOrphan()
        {
            return BrandId == null;
        }

        public override string ToString()
        {
            return Name + " = " + Value;
        }
    }
}
