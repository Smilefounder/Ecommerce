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
            IQuery query = null;

            if (String.IsNullOrEmpty(queryName))
            {
                query = QueryManager.Instance.GetQueries(typeof(ICustomerQuery)).FirstOrDefault();
                queryName = query.Name;
            }
            else
            {
                query = QueryManager.Instance.GetQuery(typeof(ICustomerQuery), queryName);
            }

            ViewBag.CurrentQuery = query;
            ViewBag.ModelType = query.ElementType;
            ViewBag.Queries = QueryManager.Instance.GetQueries(typeof(ICustomerQuery));

            var models = query.Execute(CurrentInstance, page ?? 1, pageSize ?? 50, QueryManager.Instance.GetQueryConfig(typeof(ICustomerQuery), queryName));

            return View(models);
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

        [HttpPost]
        public ActionResult Delete(CustomerModel[] model)
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

        //[HttpGet]
        //public ActionResult ExtendQuery(string name, int? page, int? pageSize)
        //{
        //    ViewBag.ExtendedQueries = EngineContext.Current.ResolveAll(typeof(CustomerQuery<>));
        //    IPagedList<CustomerRowModel> model = null;
        //    var query = _extendedQueryManager.Find<Customer>(name);
        //    if (query != null)
        //    {
        //        var paras = _extendedQueryManager.Find<Customer>(name);

        //        model = query.Execute(CurrentInstance, page ?? 1, pageSize ?? 50, paras)
        //            .Transform(o => new CustomerRowModel(o.Customer, o.OrdersCount));

        //    }
        //    else
        //    {
        //        var customerQuery = _customerService.Query();
        //        var orderQuery = _orderService.Query();
        //        model = customerQuery
        //            .GroupJoin(orderQuery,
        //                       customer => customer.Id,
        //                       order => order.CustomerId,
        //                       (customer, orders) => new { Customer = customer, Orders = orders.Count() })
        //            .OrderByDescending(groupedItem => groupedItem.Customer.Id)
        //            .ToPagedList(page, pageSize)
        //            .Transform(o => new CustomerRowModel(o.Customer, o.Orders));
        //    }
        //    return View("Index", model);
        //}

        //[HttpGet]
        //public ActionResult GetParameters(string name)
        //{
        //    var query = _extendedQueryManager.Find<ICustomerExtendedQuery>(name);
        //    var paras = _extendedQueryManager.GetExtendedQueryParameters<ICustomerExtendedQuery>(name);
        //    return JsonNet(new { Query = query, Parameters = paras });
        //}

        //[HttpPost]
        //public ActionResult SaveParameters(string name, object parameters)
        //{
        //    try
        //    {
        //        _extendedQueryManager.SaveExtendedQueryParameters<ICustomerExtendedQuery>(name, parameters);
        //        return this.JsonNet(new { status = 0, message = "Parameter Saved." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.JsonNet(new { status = 1, message = ex.Message });
        //    }
        //}
    }
}