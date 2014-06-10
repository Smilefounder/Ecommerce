using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    [Serializable]
    public class InvalidModelStateException : Exception
    {
        private ModelStateDictionary _modelState = new ModelStateDictionary();

        public ModelStateDictionary ModelState
        {
            get
            {
                return _modelState;
            }
        }

        public InvalidModelStateException(IDictionary<string, string> errors)
        {
            foreach (var each in errors)
            {
                ModelState.AddModelError(each.Key, each.Value);
            }
        }

        protected InvalidModelStateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
