using System;
using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Topbar;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers.Default;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using AutoMapper;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CustomerController : CommerceController
    {
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly CountryService _countryService;

        public CustomerController(CustomerService customerService, CountryService countryService, OrderService orderService)
        {
            _customerService = customerService;
            _countryService = countryService;
            _orderService = orderService;
        }

        public ActionResult Index()
        {
            var model = this.CreateTabQueryModel("Customers", new DefaultCustomersQuery());
            return View(model);
        }

        public ActionResult Create()
        {
            return View("Edit", new CustomerEditorModel());
        }

        public ActionResult Edit(int id)
        {
            var customer = _customerService.Find(id);
            var model = Mapper.Map<Customer, CustomerEditorModel>(customer);

            ViewBag.ToolbarCommands = TopbarCommands.GetCommands(ControllerContext, customer, CurrentInstance);

            return View(model);
        }

        [HttpGet]
        public ActionResult Get(int? id = null)
        {
            CustomerEditorModel model = null;

            if (id != null && id > 0)
            {
                var customer = _customerService.Find(id.Value);
                model = Mapper.Map<Customer, CustomerEditorModel>(customer);
            }
            else
            {
                model = new CustomerEditorModel();
            }

            return JsonNet(model).UsingClientConvention();
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void Save(CustomerEditorModel model)
        {
            Customer customer = null;

            if (model.Id > 0)
            {
                customer = _customerService.Find(model.Id);
            }
            else
            {
                customer = new Customer();
            }

            customer.Email = model.Email;

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Group = model.Group;
            customer.SavingPoints = model.SavingPoints;
            customer.Gender = model.Gender;

            customer.CustomFields.Clear();

            foreach (var field in model.CustomFields)
            {
                customer.CustomFields.Add(new CustomerCustomField(field.Name, field.Value));
            }

            if (model.Id > 0)
            {
                _customerService.Update(customer);
            }
            else
            {
                _customerService.Create(customer);
            }

            foreach (var address in customer.Addresses.ToList())
            {
                if (!model.Addresses.Any(it => it.Id == address.Id))
                {
                    customer.Addresses.Remove(address);
                }
            }

            foreach (var addressModel in model.Addresses)
            {
                var address = customer.Addresses.FirstOrDefault(it => it.Id == addressModel.Id);
                if (address == null)
                {
                    address = new Address();
                    customer.Addresses.Add(address);
                }

                UpdateAddress(address, addressModel);
            }

            _customerService.Update(customer);
        }

        private void UpdateAddress(Address address, AddressModel model)
        {
            address.FirstName = model.FirstName;
            address.LastName = model.LastName;
            address.Phone = model.Phone;
            address.Postcode = model.Postcode;
            address.Address1 = model.Address1;
            address.Address2 = model.Address2;
            address.City = model.City;
            address.State = model.State;
            address.Country = _countryService.Find(model.CountryId);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(CustomerModel[] model)
        {
            foreach (var obj in model)
            {
                var customer = _customerService.Find(obj.Id);
                _customerService.Delete(customer);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpGet]
        public ActionResult GetCountries()
        {
            var countries = _countryService.Query();
            return JsonNet(countries).UsingClientConvention();
        }
    }
}