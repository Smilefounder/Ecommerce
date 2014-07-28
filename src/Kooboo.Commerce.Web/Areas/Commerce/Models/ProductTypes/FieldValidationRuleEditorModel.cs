using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products
{
    public class FieldValidationRuleEditorModel
    {
        public FieldValidationRuleEditorModel()
        {
        }

        public FieldValidationRuleEditorModel(FieldValidationRule rule)
        {
            this.Id = rule.Id;
            this.ErrorMessage = rule.ErrorMessage;
            this.ValidatorName = rule.ValidatorName;
            this.ValidatorData = rule.ValidatorData;
        }

        public void UpdateTo(FieldValidationRule rule)
        {
            rule.Id = this.Id;
            rule.ErrorMessage = this.ErrorMessage;
            rule.ValidatorName = this.ValidatorName;
            rule.ValidatorData = this.ValidatorData;
        }

        public int Id { get; set; }

        public string ErrorMessage { get; set; }

        [Required]
        public string ValidatorName { get; set; }

        public string ValidatorData { get; set; }
    }
}