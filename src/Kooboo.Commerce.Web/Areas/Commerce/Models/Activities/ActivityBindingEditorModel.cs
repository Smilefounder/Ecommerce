using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityBindingEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public string EventClrType { get; set; }

        public string EventDisplayName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ActivityName { get; set; }

        public string ActivityDisplayName { get; set; }

        public bool IsEnabled { get; set; }

        [Required]
        public int Priority { get; set; }

        public bool IsConfigurable { get; set; }
    }
}