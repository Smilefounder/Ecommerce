using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products
{
    public class CustomFieldEditorModel
    {
        public CustomFieldEditorModel()
        {
            ControlType = "TextBox";
            ValidationRules = new List<FieldValidationRuleEditorModel>();
        }

        public CustomFieldEditorModel(CustomField field)
            : this()
        {
            Id = field.Id;
            Name = field.Name;
            DataType = field.DataType;
            Label = field.Label;
            Tooltip = field.Tooltip;
            ControlType = field.ControlType;
            DefaultValue = field.DefaultValue;
            Sequence = field.Sequence;
            IsValueLocalizable = field.IsValueLocalizable;
            IsPredefined = field.IsPredefined;
            SelectionItems = field.SelectionItems;

            foreach (var item in field.ValidationRules)
            {
                ValidationRules.Add(new FieldValidationRuleEditorModel(item));
            }
        }

        public void UpdateTo(CustomField field)
        {
            field.Name = this.Name;
            field.DataType = this.DataType;
            field.Label = this.Label;
            field.Tooltip = this.Tooltip;
            field.ControlType = this.ControlType;
            field.DefaultValue = this.DefaultValue;
            field.Sequence = this.Sequence;
            field.IsPredefined = this.IsPredefined;
            field.SelectionItems = this.SelectionItems;
            field.IsValueLocalizable = this.IsValueLocalizable;
            if (ValidationRules != null)
            {
                foreach (var rule in field.ValidationRules.ToList())
                {
                    if (!ValidationRules.Any(r => r.Id == rule.Id))
                    {
                        field.ValidationRules.Remove(rule);
                    }
                }

                foreach (var ruleModel in ValidationRules)
                {
                    FieldValidationRule rule = null;

                    if (ruleModel.Id > 0)
                    {
                        rule = field.ValidationRules.FirstOrDefault(r => r.Id == ruleModel.Id);
                    }
                    else
                    {
                        rule = new FieldValidationRule(ruleModel.ValidatorName);
                        field.ValidationRules.Add(rule);
                    }

                    rule.ValidatorName = ruleModel.ValidatorName;
                    rule.ValidatorData = ruleModel.ValidatorData;
                    rule.ErrorMessage = ruleModel.ErrorMessage;
                }
            }
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Description("Name of your field, it uses the same naming rule as C# veriable name.")]
        [RegularExpression(RegexPatterns.VeriableName, ErrorMessage = "Invalid column name.")]
        public string Name { get; set; }

        [UIHint("DropDownList")]
        [EnumDataType(typeof(FieldDataType))]
        [Required(ErrorMessage = "Required")]
        [DisplayName("Data type")]
        public FieldDataType DataType { get; set; }

        [Description("Descriptive label of your field on the product editing page.")]
        public string Label { get; set; }

        [Description("Input tip while users are editing content.")]
        public string Tooltip { get; set; }

        [Required]
        [UIHint("DropDownList")]
        [DataSource(typeof(ControlTypeDataSource))]
        [Description("The way that you would like to input your content.")]
        [DisplayName("Control type")]
        public string ControlType { get; set; }

        [Display(Name = "Default value")]
        public string DefaultValue { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Length { get; set; }

        [Description("Sequence of your field in the product editing page.")]
        [Required(ErrorMessage = "Required")]
        public int Sequence { get; set; }

        public bool IsValueLocalizable { get; set; }

        public bool IsPredefined { get; set; }

        public string SelectionItems { get; set; }

        public List<FieldValidationRuleEditorModel> ValidationRules { get; set; }
    }
}