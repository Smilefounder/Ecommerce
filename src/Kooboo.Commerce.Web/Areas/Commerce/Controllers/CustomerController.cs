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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CustomerController : CommerceControllerBase
    {
        private readonly ICommerceDatabase _db;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ICountryService _countryService;
        private readonly IExtendedQueryManager _extendedQueryManager;

        public CustomerController(ICommerceDatabase db, ICustomerService customerService, ICountryService countryService, IOrderService orderService,
            IExtendedQueryManager extendedQueryManager)
        {
            _db = db;
            _customerService = customerService;
            _countryService = countryService;
            _orderService = orderService;
            _extendedQueryManager = extendedQueryManager;
        }

        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var customerQuery = _customerService.Query();
            if (!string.IsNullOrEmpty(search))
            {
                customerQuery = customerQuery.Where(o => o.FirstName.StartsWith(search) || o.MiddleName.StartsWith(search) || o.LastName.StartsWith(search));
            }
            var orderQuery = _orderService.Query();
            var customers = customerQuery
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .OrderByDescending(groupedItem => groupedItem.Customer.Id)
                .ToPagedList(page, pageSize)
                .Transform(o => new CustomerRowModel(o.Customer, o.Orders));

            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<ExtendedQuery.CustomerQuery>();

            return View(customers);
        }

        public ActionResult Create()
        {
            //var model = new CustomerEditorModel();
            //model.GenderList = EnumUtil.ToSelectList<Gender>();
            //model.CountryList = _countryService.GetAllCountries()
            //    .ToSelectList(country => country.Name, country => country.Id.ToString(), "", "");

            //return View("Edit", model);
            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
            //var obj = _customerService.GetById(id);
            //var model = new CustomerEditorModel(obj);
            //model.GenderList = EnumUtil.ToSelectList<Gender>();
            //model.CountryList = _countryService.GetAllCountries()
            //    .ToSelectList(country => country.Name, country => country.Id.ToString(), "", "");

            //return View(model);
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
                _customerService.Save(obj);
                return this.JsonNet(new { status = 0, message = "customer succssfully saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }

        //[HttpPost]
        //public ActionResult Save(CustomerEditorModel model, string @return)
        //{
        //    return RunWithTry(data =>
        //    {
        //        var obj = new Customer();
        //        model.UpdateTo(obj);
        //        _customerService.Save(obj);
        //        data.RedirectUrl = @return;
        //    });
        //}

        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel obj)
        //{
        //    try
        //    {
        //        int status = 1;
        //        string message = null;
        //        if (obj.NewPassword != obj.ConfirmPassword)
        //        {
        //            message = "Confirm password should be the same to new password.";
        //        }
        //        else
        //        {
        //            status = _accountService.ChangePassword(obj.AccountId, obj.OldPassword, obj.NewPassword, out message) ? 0 : 1;
        //        }

        //        return this.JsonNet(new { status = status, message = message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.JsonNet(new { status = 1, message = ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult Delete(CustomerRowModel[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ =>
            {
                foreach (var obj in model)
                {
                    var customer = _customerService.GetById(obj.Id);
                    _customerService.Delete(customer);
                }
                data.ReloadPage = true;
            });

            return Json(data);
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

            var objs = _orderService.Query().Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.Id)
                .ToPagedList(page, pageSize);
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult ExtendQuery(string name, int? page, int? pageSize)
        {
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<ExtendedQuery.CustomerQuery>();
            IPagedList<CustomerRowModel> model = null;
            var query = _extendedQueryManager.GetExtendedQuery<ExtendedQuery.CustomerQuery>(name);
            if (query != null)
            {
                var paras = _extendedQueryManager.GetExtendedQueryParameters<ExtendedQuery.CustomerQuery>(name);

                model = query.Query(paras, _db, page ?? 1, pageSize ?? 50)
                    .Transform(o => new CustomerRowModel(o.Customer, o.OrdersCount));

            }
            else
            {
                var customerQuery = _customerService.Query();
                var orderQuery = _orderService.Query();
                model = customerQuery
                    .GroupJoin(orderQuery,
                               customer => customer.Id,
                               order => order.CustomerId,
                               (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                    .OrderByDescending(groupedItem => groupedItem.Customer.Id)
                    .ToPagedList(page, pageSize)
                    .Transform(o => new CustomerRowModel(o.Customer, o.Orders));
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult GetParameters(string name)
        {
            var query = _extendedQueryManager.GetExtendedQuery<ExtendedQuery.CustomerQuery>(name);
            var paras = _extendedQueryManager.GetExtendedQueryParameters<ExtendedQuery.CustomerQuery>(name);
            return JsonNet(new { Query = query, Parameters = paras });
        }

        [HttpPost]
        public ActionResult SaveParameters(string name, IEnumerable<ExtendedQueryParameter> parameters)
        {
            try
            {
                _extendedQueryManager.SaveExtendedQueryParameters<ExtendedQuery.CustomerQuery>(name, parameters);
                return this.JsonNet(new { status = 0, message = "Parameter Saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
    }
}