using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.ComponentModel.DataAnnotations
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        public string DependentProperty { get; set; }

        public object DependentPropertyValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object dependentPropertyValue)
        {
            DependentProperty = dependentProperty;
            DependentPropertyValue = dependentPropertyValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var otherProperty = containerType.GetProperty(DependentProperty);

            if (otherProperty == null)
                throw new InvalidOperationException("Dependent property \"" + DependentProperty + "\" was not found.");

            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
            if ((otherPropertyValue == null && DependentPropertyValue == null)
                || (otherPropertyValue != null && otherPropertyValue.Equals(DependentPropertyValue)))
            {
                return base.IsValid(value, validationContext);
            }

            return ValidationResult.Success;
        }
    }
}
