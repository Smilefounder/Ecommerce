using Kooboo.CMS.Sites.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    public abstract class SubmissionPluginBase<TModel> : SubmissionPluginBase
        where TModel : class
    {
        protected override object Execute()
        {
            var model = (TModel)Activator.CreateInstance(typeof(TModel));
            if (!ModelBindHelper.BindModel<TModel>(model, "", ControllerContext, SubmissionSetting))
            {
                throw new InvalidModelStateException("Invalid model state.");
            }

            return Execute(model);
        }

        protected abstract object Execute(TModel model);
    }
}
