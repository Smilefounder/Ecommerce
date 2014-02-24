using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.EAV {

    public class CustomField {

        public CustomField() {
            FieldType = CustomFieldType.Custom;
            ValidationRules = new List<FieldValidationRule>();
        }

        public void CopyTo(CustomField field) {
            field.Name = this.Name;
            field.FieldType = this.FieldType;
            field.DataType = this.DataType;
            field.Label = this.Label;
            field.Tooltip = this.Tooltip;
            field.ControlType = this.ControlType;
            field.DefaultValue = this.DefaultValue;
            field.Length = this.Length;
            field.Sequence = this.Sequence;
            field.Modifiable = this.Modifiable;
            field.Indexable = this.Indexable;
            field.AllowNull = this.AllowNull;
            field.ShowInGrid = this.ShowInGrid;
            field.Summarize = this.Summarize;
            field.IsEnabled = this.IsEnabled;
            field.SelectionItems = this.SelectionItems;
            field.CustomSettings = this.CustomSettings;
        }

        public CustomField DeepClone() {
            return (CustomField)this.MemberwiseClone();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CustomFieldType FieldType { get; set; }

        [Required]
        public FieldDataType DataType { get; set; }

        [StringLength(50)]
        public string Label { get; set; }

        [StringLength(1000)]
        public string Tooltip { get; set; }

        [Required, StringLength(50)]
        public string ControlType { get; set; }

        public string DefaultValue { get; set; }

        public int Length { get; set; }

        public int Sequence { get; set; }

        public bool Modifiable { get; set; }

        public bool Indexable { get; set; }

        public bool AllowNull { get; set; }

        public bool ShowInGrid { get; set; }

        public bool Summarize { get; set; }

        public bool IsEnabled { get; set; }

        public string CustomSettings { get; set; }

        public string SelectionItems { get; set; }

        public virtual ProductType ByCustomFields { get; set; }

        public virtual ProductType ByVariationFields { get; set; }

        public virtual ICollection<FieldValidationRule> ValidationRules { get; set; }
    }
}
