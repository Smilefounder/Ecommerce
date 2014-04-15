using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    [Grid(IdProperty = "EventType")]
    public class ActivityEventRowModel
    {
        [Key]
        public string EventType { get; set; }

        [LinkedGridColumn(TargetAction = "List")]
        public string Name { get; set; }
    }
}