using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{

    public class ProductSettingEditorModel
    {
        public List<CustomFieldEditorModel> PredefinedFields { get; set; }

        public ProductSettingEditorModel()
        {
            PredefinedFields = new List<CustomFieldEditorModel>();
        }

        public ProductSettingEditorModel(IEnumerable<CustomField> fields)
            : this()
        {
            foreach (var field in fields)
            {
                PredefinedFields.Add(new CustomFieldEditorModel(field));
            }
        }
    }
}