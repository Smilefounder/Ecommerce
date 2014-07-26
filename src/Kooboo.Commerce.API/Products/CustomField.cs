using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.EAV
{
    /// <summary>
    /// custom field base on entity attribute value
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// custom field id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// custom field name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// field type
        /// </summary>
        public CustomFieldType FieldType { get; set; }
        /// <summary>
        /// data type
        /// </summary>
        public FieldDataType DataType { get; set; }
        /// <summary>
        /// the label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// tooltip for explaining the custom field
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// the control type of the custom field. this will determine the editor of the custom field
        /// </summary>
        public string ControlType { get; set; }
        /// <summary>
        /// default value
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// the length of the input
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// the order(sequence) when displaying the custom fields
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// is this custom field modifiable or read only
        /// </summary>
        public bool Modifiable { get; set; }
        /// <summary>
        /// is this custom field indexable 
        /// </summary>
        public bool Indexable { get; set; }
        /// <summary>
        /// allow the custom field be null
        /// </summary>
        public bool AllowNull { get; set; }
        /// <summary>
        /// show this custom field in the list grid
        /// </summary>
        public bool ShowInGrid { get; set; }
        /// <summary>
        /// is this custom field a summery of the object
        /// </summary>
        public bool Summarize { get; set; }
        /// <summary>
        /// enabled 
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// custom settings 
        /// </summary>
        public string CustomSettings { get; set; }
        /// <summary>
        /// selection items.
        /// when the editor is a dropdown list, get the options from here.
        /// </summary>
        public string SelectionItems { get; set; }
        /// <summary>
        /// validation rules for the custom field
        /// </summary>
        public FieldValidationRule[] ValidationRules { get; set; }
    }
}
