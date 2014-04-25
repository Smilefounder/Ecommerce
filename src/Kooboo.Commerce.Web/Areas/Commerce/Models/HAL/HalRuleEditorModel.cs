using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.API.HAL;
using Kooboo.ComponentModel;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.HAL
{
    public class HalRuleEditorModel
    {
         public HalRuleEditorModel() {
        }

         public HalRuleEditorModel(HalRule rule)
         {
            this.Id = rule.Id;
            this.Name = rule.Name;
            this.ConditionsExpression = rule.ConditionsExpression;
            this.Resources = new List<HalRuleResourceModel>();
            if (rule.Resources != null && rule.Resources.Count > 0)
            {
                foreach (var cf in rule.Resources)
               {
                   var cfm = new HalRuleResourceModel(cf);
                   this.Resources.Add(cfm);
               }
            }
        }

         public void UpdateTo(HalRule rule)
         {
            rule.Id = this.Id;
            rule.Name = (this.Name ?? string.Empty).Trim();
            rule.ConditionsExpression = (this.ConditionsExpression ?? string.Empty).Trim();

            if (this.Resources != null && this.Resources.Count > 0)
            {
                rule.Resources = new List<HalRuleResource>();
                foreach (var cfm in this.Resources)
                {

                    var cf = new HalRuleResource();
                    cfm.UpdateTo(cf);

                    rule.Resources.Add(cf);
                }
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ConditionsExpression { get; set; }

        public ICollection<HalRuleResourceModel> Resources { get; set; }
   }
}