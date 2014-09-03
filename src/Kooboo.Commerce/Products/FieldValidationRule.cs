using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class FieldValidationRule : IOrphanable
    {
        public FieldValidationRule() { }

        public FieldValidationRule(string validatorName)
        {
            ValidatorName = validatorName;
        }

        public int Id { get; set; }

        [StringLength(500)]
        public string ErrorMessage { get; set; }

        [Required, StringLength(50)]
        public string ValidatorName { get; set; }

        public string ValidatorConfig { get; set; }

        [Column]
        protected int? CustomFieldId { get; set; }

        public void UpdateFrom(FieldValidationRule other)
        {
            ErrorMessage = other.ErrorMessage;
            ValidatorName = other.ValidatorName;
            ValidatorConfig = other.ValidatorConfig;
        }

        bool IOrphanable.IsOrphan()
        {
            return CustomFieldId == null;
        }
    }
}
