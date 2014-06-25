using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Pricing;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class ShippingPlugin : ISubmissionPlugin
    {
        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }

        public System.Web.Mvc.ActionResult Submit(Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Sites.Models.SubmissionSetting submissionSetting)
        {
            var request = controllerContext.HttpContext.Request;
            var action = request["action"];

            var jsonResultData = new JsonResultData();
            object result = null;

            try
            {
                if (action == "get-addresses")
                {
                    result = GetAddresses(site, controllerContext);
                }

                jsonResultData.Success = true;
                jsonResultData.Model = result;
            }
            catch (Exception ex)
            {
                jsonResultData.Success = false;
                jsonResultData.AddException(ex);
            }

            return new JsonResult { Data = jsonResultData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private ShippingAddressesModel GetAddresses(Site site, ControllerContext controllerContext)
        {
            var model = new ShippingAddressesModel();
            var member = controllerContext.HttpContext.Membership().GetMembershipUser();
            var customer = site.Commerce().Customers
                                          .ByAccountId(member.UUID)
                                          .Include(c => c.Addresses)
                                          .FirstOrDefault();

            Address defaultAddr = null;

            if (customer.ShippingAddressId != null)
            {
                defaultAddr = customer.Addresses.FirstOrDefault(a => a.Id == customer.ShippingAddressId.Value);
            }

            if (defaultAddr == null)
            {
                defaultAddr = customer.Addresses.FirstOrDefault();
            }

            model.Default = defaultAddr;
            model.Alternatives = customer.Addresses.Where(x => x.Id != defaultAddr.Id).ToList();

            return model;
        }
    }
}
