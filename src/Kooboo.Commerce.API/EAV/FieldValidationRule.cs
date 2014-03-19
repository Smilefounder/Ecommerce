using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.EAV
{
    public class FieldValidationRule
    {
        public int Id { get; set; }

        public string ErrorMessage { get; set; }

        public string ValidatorName { get; set; }

        public string ValidatorData { get; set; }
    }
}
