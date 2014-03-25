using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.EAV
{
    /// <summary>
    /// validation rule for the custom field
    /// </summary>
    public class FieldValidationRule
    {
        /// <summary>
        /// rule id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// error message when the field value is not validated
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// validator name
        /// </summary>
        public string ValidatorName { get; set; }
        /// <summary>
        /// validator data
        /// </summary>
        public string ValidatorData { get; set; }
    }
}
