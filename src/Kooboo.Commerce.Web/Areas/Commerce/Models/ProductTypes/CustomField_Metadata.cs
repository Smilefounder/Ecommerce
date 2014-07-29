using Kooboo.Commerce.Products;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{
    [MetadataFor(typeof(CustomField))]
    public class CustomField_Metadata
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Label { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(ControlTypeDataSource))]
        public string ControlType { get; set; }

        [Display(Name = "Default value")]
        public string DefaultValue { get; set; }

        [Display(Name = "Is value localizable")]
        public bool IsValueLocalizable { get; set; }
    }
}