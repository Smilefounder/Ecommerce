using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class ProductTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CustomFieldDefinitionModel> CustomFieldDefinitions { get; set; }

        public IList<CustomFieldDefinitionModel> VariantFieldDefinitions { get; set; }

        public ProductTypeModel()
        {
            CustomFieldDefinitions = new List<CustomFieldDefinitionModel>();
            VariantFieldDefinitions = new List<CustomFieldDefinitionModel>();
        }
    }

    public class CustomFieldDefinitionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string ControlType { get; set; }

        public string DefaultValue { get; set; }

        public IList<SelectionItem> SelectionItems { get; set; }

        public CustomFieldDefinitionModel()
        {
            SelectionItems = new List<SelectionItem>();
        }
    }
}