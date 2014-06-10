using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.View;
using System.Web;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    public abstract class SubmissionPluginBase<TModel> : ISubmissionPlugin
        where TModel : SubmissionModel, new()
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

        protected ICommerceAPI Api
        {
            get
            {
                return Site.Commerce();
            }
        }

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

            var model = new TModel();

            if (!ModelBindHelper.BindModel<TModel>(model, "", ControllerContext, SubmissionSetting))
            {
                var resultData = new JsonResultData();
                resultData.Success = false;
                resultData.AddModelState(ControllerContext.Controller.ViewData.ModelState);

                ClearTempFields();

                return new JsonResult { Data = resultData };
            }

            try
            {
                var result = Execute(model);

                string redirectUrl = null;

                if (result != null)
                {
                    redirectUrl = result.RedirectUrl;
                }

                if (String.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = ResolveUrl(model.SuccessUrl, controllerContext);
                }

                if (!String.IsNullOrEmpty(redirectUrl))
                {
                    return RedirectTo(redirectUrl, result == null ? null : result.Data);
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new JsonResultData
                        {
                            Success = true,
                            Model = result == null ? null : result.Data
                        }
                    };
                }
            }
            catch (InvalidModelStateException ex)
            {
                var resultData = new JsonResultData();
                resultData.Success = false;
                resultData.AddModelState(ex.ModelState);

                return new JsonResult { Data = resultData };
            }
            catch (Exception ex)
            {
                Kooboo.HealthMonitoring.Log.LogException(ex);

                var redirectUrl = ResolveUrl(model.FailedUrl, ControllerContext);
                if (!String.IsNullOrEmpty(redirectUrl))
                {
                    return RedirectTo(redirectUrl, null);
                }
                else
                {
                    var resultData = new JsonResultData();
                    resultData.Success = false;
                    resultData.AddException(ex);

                    return new JsonResult { Data = resultData };
                }
            }
            finally
            {
                ClearTempFields();
            }
        }

        private ActionResult RedirectTo(string redirectUrl, object data)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return new JsonResult
                {
                    Data = new JsonResultData
                    {
                        RedirectUrl = redirectUrl,
                        Model = data
                    }
                };
            }

            return new RedirectResult(redirectUrl);
        }

        private void ClearTempFields()
        {
            Site = null;
            ControllerContext = null;
            SubmissionSetting = null;
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

        protected abstract SubmissionExecuteResult Execute(TModel model);
    }

    public abstract class SubmissionPluginBase : SubmissionPluginBase<SubmissionModel>
    {
    }
}
