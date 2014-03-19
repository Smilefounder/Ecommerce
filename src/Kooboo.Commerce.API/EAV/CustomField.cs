using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Products;

namespace Kooboo.Commerce.API.EAV
{

    public class CustomField
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CustomFieldType FieldType { get; set; }

        public FieldDataType DataType { get; set; }

        public string Label { get; set; }

        public string Tooltip { get; set; }

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

        public ProductType ByCustomFields { get; set; }

        public ProductType ByVariationFields { get; set; }

        public FieldValidationRule[] ValidationRules { get; set; }
    }
}
