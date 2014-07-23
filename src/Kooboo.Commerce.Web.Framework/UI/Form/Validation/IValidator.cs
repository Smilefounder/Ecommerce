using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Form.Validation
{
    public interface IValidator
    {
        string Name { get; }

        IEnumerable<ModelClientValidationRule> GetClientValidationRules(CustomField field, FieldValidationRule rule);
    }

    public static class ControlValidators
    {
        public static IEnumerable<IValidator> Validators()
        {
            return EngineContext.Current.ResolveAll<IValidator>();
        }
    }
}
