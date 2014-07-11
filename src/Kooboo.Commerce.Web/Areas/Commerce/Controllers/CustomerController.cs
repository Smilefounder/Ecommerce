using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CustomerController : CommerceControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ICountryService _countryService;

        public CustomerController(ICustomerService customerService, ICountryService countryService, IOrderService orderService)
        {
            _customerService = customerService;
            _countryService = countryService;
            _orderService = orderService;
        }

        public ActionResult Index(string queryName, int? page, int? pageSize)
        {
            var model = new QueryGridModel
            {
                AllQueryInfos = QueryManager.Instance.GetQueryInfos(QueryTypes.Customers).ToList()
            };

            if (String.IsNullOrEmpty(queryName))
            {
                model.CurrentQueryInfo = model.AllQueryInfos.FirstOrDefault();
            }
            else
            {
                model.CurrentQueryInfo = QueryManager.Instance.GetQueryInfo(queryName);
            }

            model.CurrentQueryResult = model.CurrentQueryInfo.Query.Execute(CurrentInstance, page ?? 1, pageSize ?? 50, model.CurrentQueryInfo.GetQueryConfig());

            return View(model);
        }

        public ActionResult Create()
        {
            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Get(int? id = null)
        {
            var obj = id == null ? null : _customerService.GetById(id.Value);
            if (obj == null)
            {
                obj = new Customer();
            }
            return JsonNet(obj);
        }

        [HttpPost, Transactional]
        public ActionResult Save(Customer obj)
        {
            try
            {
                if (obj.Id > 0)
                {
                    _customerService.Update(obj);
                }
                else
                {
                    _customerService.Create(obj);
                }

                return this.JsonNet(new { status = 0, message = "customer succssfully saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(CustomerModel[] model)
        {
            foreach (var obj in model)
            {
                var customer = _customerService.GetById(obj.Id);
                _customerService.Delete(customer);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpGet]
        public ActionResult GetCountries()
        {

            var objs = _countryService.Query();
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult GetOrders(int customerId, int? page, int? pageSize)
        {
            var objs = _orderService.Query()
                                    .Where(o => o.CustomerId == customerId)
                                    .OrderByDescending(o => o.Id)
                                    .ToPagedList(page, pageSize);
            return JsonNet(objs);
        }
    }
}