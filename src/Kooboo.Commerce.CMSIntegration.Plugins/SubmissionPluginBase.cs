using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.View;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    public abstract class SubmissionPluginBase : ISubmissionPlugin
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public Dictionary<string, object> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        protected ControllerContext ControllerContext { get; private set; }

        protected HttpContextBase HttpContext
        {
            get
            {
                return ControllerContext == null ? null : ControllerContext.HttpContext;
            }
        }

        protected Site Site { get; private set; }

        protected SubmissionSetting SubmissionSetting { get; private set; }

        protected SubmissionPluginBase()
        {
            _parameters.Add("SuccessUrl", "");
            _parameters.Add("FailedUrl", "");
        }

        public System.Web.Mvc.ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            // Setup temp fields

            Site = site;
            ControllerContext = controllerContext;
            SubmissionSetting = submissionSetting;

            var resultData = new JsonResultData();

            try
            {
                var result = Execute();
                var redirectUrl = ResolveUrl(controllerContext.HttpContext.Request["SuccessUrl"], controllerContext);

                if (!String.IsNullOrEmpty(redirectUrl))
                {
                    return new RedirectResult(redirectUrl);
                }
                else
                {
                    resultData.Success = true;
                    resultData.Model = result;
                }
            }
            catch (Exception ex)
            {
                Kooboo.HealthMonitoring.Log.LogException(ex);

                var redirectUrl = ResolveUrl(HttpContext.Request["FailedUrl"], ControllerContext);
                if (!String.IsNullOrEmpty(redirectUrl))
                {
                    return new RedirectResult(redirectUrl);
                }
                else
                {
                    resultData.Success = false;
                    resultData.AddException(ex);
                }
            }
            finally
            {
                // Cleanup temp fields
                Site = null;
                ControllerContext = null;
                SubmissionSetting = null;
            }

            return new JsonResult
            {
                Data = resultData
            };
        }

        protected string ResolveUrl(string url, ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            url = UrlHelper.GenerateContentUrl(url, controllerContext.HttpContext);

            return controllerContext.RequestContext.UrlHelper().FrontUrl().WrapperUrl(url).ToString();
        }

        protected abstract object Execute();
    }
}
