using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.EAV {

    public class CustomFieldEditorModel {

        public CustomFieldEditorModel() {
            this.FieldType = CustomFieldType.Custom;
            this.ValidationRules = new List<FieldValidationRuleEditorModel>();
        }

        public CustomFieldEditorModel(CustomField field)
            : this() {
            this.Id = field.Id;
            this.Name = field.Name;
            this.DataType = field.DataType;
            this.Label = field.Label;
            this.Tooltip = field.Tooltip;
            this.ControlType = field.ControlType;
            this.DefaultValue = field.DefaultValue;
            this.Length = field.Length;
            this.Sequence = field.Sequence;
            this.Modifiable = field.Modifiable;
            this.Indexable = field.Indexable;
            this.AllowNull = field.AllowNull;
            this.ShowInGrid = field.ShowInGrid;
            this.Summarize = field.Summarize;
            this.IsEnabled = field.IsEnabled;
            this.FieldType = field.FieldType;
            this.CustomSettings = field.CustomSettings;
            this.SelectionItems = field.SelectionItems;
            if (field.ValidationRules != null) {
                foreach (var item in field.ValidationRules) {
                    this.ValidationRules.Add(new FieldValidationRuleEditorModel(item));
                }
            }
        }

        public void UpdateTo(CustomField field) {
            field.Id = this.Id;
            field.Name = this.Name;
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
            field.FieldType = this.FieldType;
            field.CustomSettings = this.CustomSettings;
            field.SelectionItems = this.SelectionItems;
            field.ValidationRules = new List<FieldValidationRule>();
            if (this.ValidationRules != null) {
                foreach (var item in this.ValidationRules) {
                    var rule = new FieldValidationRule();
                    item.UpdateTo(rule);
                    field.ValidationRules.Add(rule);
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

        [Description("Descriptive label of your field on the content editing page.")]
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

        [Description("Sequence of your field in the content editing page.")]
        [Required(ErrorMessage = "Required")]
        public int Sequence { get; set; }

        [Description("Enable the modification of this field in the content editing page.")]
        [DefaultValue(true)]
        [Required(ErrorMessage = "Required")]
        public bool Modifiable { get; set; }

        [Description("Include this field in the Lucene.NET full text search index.")]
        [Required(ErrorMessage = "Required")]
        public bool Indexable { get; set; }

        [DisplayName("Allow null")]
        [Description("Allows null value in this field")]
        [Required(ErrorMessage = "Required")]
        public bool AllowNull { get; set; }

        [Description("Show this field in the content list page.")]
        [Display(Name = "Content list page")]
        [Required(ErrorMessage = "Required")]
        public bool ShowInGrid { get; set; }

        [Display(Name = "Summary field")]
        [Description("The Summary field will be used as a title or summary to describe your content item.")]
        [Required(ErrorMessage = "Required")]
        public bool Summarize { get; set; }

        public bool IsEnabled { get; set; }

        public CustomFieldType FieldType { get; set; }

        [Description("The other custom settings for the field.")]
        public string CustomSettings { get; set; }

        public string SelectionItems { get; set; }

        public List<FieldValidationRuleEditorModel> ValidationRules { get; set; }
    }
}