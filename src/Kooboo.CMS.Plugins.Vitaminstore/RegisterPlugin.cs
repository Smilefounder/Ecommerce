using Kooboo.CMS.Common;
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Commerce.API.CmsSite;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class RegisterPlugin : ISubmissionPlugin
    {
        private MembershipUserManager _manager;

        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }

        public RegisterPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }

        public System.Web.Mvc.ActionResult Submit(Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Sites.Models.SubmissionSetting submissionSetting)
        {
            var result = new JsonResultData();
            var model = new RegisterModel();

            var valid = ModelBindHelper.BindModel(model, "", controllerContext, submissionSetting);

            if (!valid)
            {
                result.Success = false;
                result.AddModelState(controllerContext.Controller.ViewData.ModelState);
                return new JsonResult
                {
                    Data = result
                };
            }

            try
            {
                var membership = MemberPluginHelper.GetMembership();
                var user = _manager.Create(membership, model.Email, model.Email, model.Password, true, "en-US", null);

                var customer = new Customer
                {
                    AccountId = user.UUID,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender),

                    Phone = model.Mobile,
                    City = model.City,
                    CountryId = model.Country
                };

                customer.CustomFields.Add(new CustomerCustomField("ClubCard", model.ClubCard.ToString().ToLower()));
                customer.CustomFields.Add(new CustomerCustomField("ClubCardNumber", model.ClubCardNumber));
                customer.CustomFields.Add(new CustomerCustomField("ClubCardPostcode", model.ClubCardPostcode));

                customer.CustomFields.Add(new CustomerCustomField("Tel", model.Tel));
                customer.CustomFields.Add(new CustomerCustomField("BankNumber", model.BankNumber));

                customer.CustomFields.Add(new CustomerCustomField("WhereKnowUsFrom", model.WhereKnowUsFrom));
                customer.CustomFields.Add(new CustomerCustomField("AcceptNewsletter", model.AcceptNewsletter.ToString().ToLower()));

                var address = new Address
                {
                    Postcode = model.Postcode,
                    Address1 = model.Street,
                    Address2 = model.HouseNumber + " " + model.HouseNumberAddition,
                    City = model.City,
                    CountryId = model.Country
                };

                customer.Addresses.Add(address);

                site.Commerce().Customers.Create(customer);

                var auth = new MembershipAuthentication(site, membership, controllerContext.HttpContext);
                auth.SetAuthCookie(customer.Email, false);

                result.Success = true;

                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                result.AddException(ex);
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
