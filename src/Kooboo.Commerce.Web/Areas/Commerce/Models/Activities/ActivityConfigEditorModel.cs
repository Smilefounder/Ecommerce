using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityConfigEditorModel
    {
        public int RuleId { get; set; }

        public int AttachedActivityInfoId { get; set; }

        public object Config { get; set; }
    }
}