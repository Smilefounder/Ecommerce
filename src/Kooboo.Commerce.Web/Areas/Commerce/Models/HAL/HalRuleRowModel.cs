using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    [Grid(Checkable = true, IdProperty = "Id")]
    public class HalRuleRowModel
    {
        [LinkedGridColumn(TargetAction = "EditRule", HeaderText = "Rule Id")]
        public int Id { get; set; }

        [GridColumn]
        public string Name { get; set; }
        
        public HalRuleRowModel() { }

         public HalRuleRowModel(HalRule rule)
        {
            Id = rule.Id;
            Name = rule.Name;
        }
   }
}