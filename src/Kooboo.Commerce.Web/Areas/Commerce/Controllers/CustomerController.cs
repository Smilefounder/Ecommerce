using System;
using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Customers;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Customers.Default;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;

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

        public ActionResult Index(string search, string queryId, int page = 1, int pageSize = 50)
        {
            var manager = new SavedTabQueryManager();
            var model = new TabQueryModel
            {
                PageName = "Customers",
                SavedQueries = manager.FindAll("Customers").ToList(),
                AvailableQueries = TabQueries.GetQueries(ControllerContext).ToList()
            };

            // Ensure default
            if (model.SavedQueries.Count == 0)
            {
                var savedQuery = SavedTabQuery.CreateFrom(new DefaultCustomersQuery(), "All");
                manager.Add(model.PageName, savedQuery);
                model.SavedQueries.Add(savedQuery);
            }

            if (String.IsNullOrEmpty(queryId))
            {
                model.CurrentQuery = model.SavedQueries.FirstOrDefault();
            }
            else
            {
                model.CurrentQuery = manager.Find(model.PageName, new Guid(queryId));
            }

            var query = model.AvailableQueries.Find(q => q.Name == model.CurrentQuery.QueryName);

            model.CurrentQueryResult = query.Execute(new QueryContext(CurrentInstance, search, page - 1, pageSize, model.CurrentQuery.Config))
                                            .ToPagedList();

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult ExecuteToolbarCommand(string commandName, CustomerModel[] model, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config = null)
        {
            var command = ToolbarCommands.GetCommand(commandName);
            var customers = model.Select(m => _customerService.GetById(m.Id)).ToList();
            var results = ToolbarCommandHelper.SafeExecute(command, config, customers, CommerceInstance.Current);

            command.SetDefaultCommandConfig(config);

            return AjaxForm().WithModel(results).ReloadPage();
        }

        public ActionResult Create()
        {
            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetById(id);

            ViewBag.ToolbarCommands = ToolbarCommands.GetCommands(ControllerContext, customer, CurrentInstance);

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
        public ActionResult GetOrders(int customerId, int page = 1, int pageSize = 50)
        {
            var objs = _orderService.Query()
                                    .Where(o => o.CustomerId == customerId)
                                    .OrderByDescending(o => o.Id)
                                    .Paginate(page - 1, pageSize)
                                    .ToPagedList();
            return JsonNet(objs);
        }
    }
}