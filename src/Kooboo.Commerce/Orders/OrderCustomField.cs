using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Orders
{
    public class OrderCustomField : IOrphanable
    {
        [Key]
        protected int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; protected set; }

        public string Value { get; set; }

        [Column]
        protected int? OrderId { get; set; }

        protected OrderCustomField() { }

        public OrderCustomField(string name, string value)
        {
            Require.NotNullOrEmpty(name, "name");
            Name = name;
            Value = value;
        }

        public bool IsOrphan()
        {
            return OrderId == null;
        }

        public override string ToString()
        {
            return Name + " = " + Value;
        }
    }
}
