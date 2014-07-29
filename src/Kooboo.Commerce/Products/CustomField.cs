using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Products
{
    public class CustomField
    {
        public CustomField()
        {
            ControlType = "TextBox";
            ValidationRules = new List<FieldValidationRule>();
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public bool IsPredefined { get; set; }

        [StringLength(50)]
        public string Label { get; set; }

        [StringLength(1000)]
        public string Tooltip { get; set; }

        [Required, StringLength(50)]
        public string ControlType { get; set; }

        public string ControlConfig { get; set; }

        [StringLength(1000)]
        public string DefaultValue { get; set; }

        public int Sequence { get; set; }

        public bool IsValueLocalizable { get; set; }

        public virtual ICollection<FieldValidationRule> ValidationRules { get; set; }

        public void UpdateFrom(CustomField field)
        {
            // Do not update IsPredefined and Id
            Name = field.Name;
            Label = field.Label;
            Tooltip = field.Tooltip;
            ControlType = field.ControlType;
            DefaultValue = field.DefaultValue;
            Sequence = field.Sequence;
            IsValueLocalizable = field.IsValueLocalizable;
            ControlConfig = field.ControlConfig;

            ValidationRules.Update(
                from: field.ValidationRules,
                by: r => r.Id,
                onUpdateItem: (oldItem, newItem) => oldItem.UpdateFrom(newItem));
        }
    }
}
