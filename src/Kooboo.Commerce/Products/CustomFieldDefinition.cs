using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Data;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Kooboo.Commerce.Products
{
    public class CustomFieldDefinition
    {
        public CustomFieldDefinition()
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

        [Column("SelectionItems")]
        protected string SelectionItemsJson { get; set; }

        private List<SelectionItem> _selectionItems;

        [NotMapped]
        public IEnumerable<SelectionItem> SelectionItems
        {
            get
            {
                if (_selectionItems == null)
                {
                    if (String.IsNullOrWhiteSpace(SelectionItemsJson))
                    {
                        _selectionItems = new List<SelectionItem>();
                    }
                    else
                    {
                        _selectionItems = JsonConvert.DeserializeObject<List<SelectionItem>>(SelectionItemsJson);
                    }
                }

                return _selectionItems;
            }
            set
            {
                if (value == null)
                {
                    SelectionItemsJson = null;
                }
                else
                {
                    SelectionItemsJson = JsonConvert.SerializeObject(value);
                }

                _selectionItems = null;
            }
        }

        [StringLength(1000)]
        public string DefaultValue { get; set; }

        public int Sequence { get; set; }

        public bool IsValueLocalizable { get; set; }

        public virtual ICollection<FieldValidationRule> ValidationRules { get; set; }

        public void UpdateFrom(CustomFieldDefinition field)
        {
            // Do not update IsPredefined and Id
            Name = field.Name;
            Label = field.Label;
            Tooltip = field.Tooltip;
            ControlType = field.ControlType;
            DefaultValue = field.DefaultValue;
            Sequence = field.Sequence;
            IsValueLocalizable = field.IsValueLocalizable;
            SelectionItems = field.SelectionItems;

            ValidationRules.Update(
                from: field.ValidationRules,
                by: r => r.Id,
                onUpdateItem: (oldItem, newItem) => oldItem.UpdateFrom(newItem));
        }
    }
}
