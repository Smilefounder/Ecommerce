using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV
{
    public class FieldValidationRule
    {
        protected FieldValidationRule() { }

        public FieldValidationRule(string validatorName)
        {
            ValidatorName = validatorName;
        }

        public void CopyTo(FieldValidationRule rule)
        {
            rule.ErrorMessage = ErrorMessage;
            rule.ValidatorName = ValidatorName;
            rule.ValidatorData = ValidatorData;
        }

        public int Id { get; set; }

        [StringLength(1000)]
        public string ErrorMessage { get; set; }

        [Required, StringLength(50)]
        public string ValidatorName { get; set; }

        public string ValidatorData { get; set; }
    }
}
