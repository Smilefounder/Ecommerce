using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ShipmentTrackers
{
    [Grid(IdProperty = "Name")]
    public class ShipmentTrackerRowModel
    {
        [Key]
        public string Name { get; set; }

        [LinkedGridColumn(TargetAction = "Settings", HeaderText = "Name")]
        public string DisplayName { get; set; }

        [GridColumn(HeaderText = "Supported carriers")]
        public string SupportedCarriers { get; set; }
    }
}