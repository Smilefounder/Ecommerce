using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV
{
    public class FieldValidationRule
    {
        public void CopyTo(FieldValidationRule rule) {
            rule.ErrorMessage = this.ErrorMessage;
            rule.ValidatorName = this.ValidatorName;
            rule.ValidatorData = this.ValidatorData;
        }

        public int Id { get; set; }

        [StringLength(1000)]
        public string ErrorMessage { get; set; }

        [Required, StringLength(50)]
        public string ValidatorName { get; set; }

        public string ValidatorData { get; set; }
    }
}
