using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Areas.Commerce.Models.EAV;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{

    public class ProductSettingEditorModel
    {
        public List<CustomFieldEditorModel> SystemFields { get; set; }

        public ProductSettingEditorModel()
        {
            SystemFields = new List<CustomFieldEditorModel>();
        }

        public ProductSettingEditorModel(IEnumerable<CustomField> systemFields)
            : this()
        {
            foreach (var field in systemFields)
            {
                SystemFields.Add(new CustomFieldEditorModel(field));
            }
        }
    }
}