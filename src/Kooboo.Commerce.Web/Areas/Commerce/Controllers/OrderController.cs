using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Queries.Orders;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using Kooboo.Commerce.Web.Mvc.ModelBinding;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class OrderController : CommerceControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ICountryService _countryService;
        private readonly IPaymentMethodService _paymentMethodService;

        public OrderController(IOrderService orderService, ICustomerService customerService, IProductService productService, ICountryService countryService, IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _countryService = countryService;
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public ActionResult Index(string search, string queryId, int page = 1, int pageSize = 50)
        {
            var manager = new SavedTabQueryManager();

            var model = new TabQueryModel
            {
                PageName = "Orders",
                SavedQueries = manager.GetSavedQueries("Orders").ToList(),
                AvailableQueries = TabQueries.GetQueries(ControllerContext).ToList()
            };

            if (model.SavedQueries.Count == 0)
            {
                var savedQuery = SavedTabQuery.CreateFrom(model.AvailableQueries.First());
                manager.AddSavedQuery(model.PageName, savedQuery);
                model.SavedQueries.Add(savedQuery);
            }

            if (String.IsNullOrEmpty(queryId))
            {
                model.CurrentQuery = model.SavedQueries.FirstOrDefault();
            }
            else
            {
                model.CurrentQuery = manager.GetSavedQuery(model.PageName, new Guid(queryId));
            }

            var query = model.AvailableQueries.Find(q => q.Name == model.CurrentQuery.QueryName);

            model.CurrentQueryResult = query.Execute(new QueryContext(CurrentInstance, search, page - 1, pageSize, model.CurrentQuery.Config))
                                            .ToPagedList();

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult ExecuteToolbarCommand(string commandName, OrderModel[] model, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config = null)
        {
            var command = ToolbarCommands.GetCommand(commandName);
            var orders = model.Select(m => _orderService.GetById(m.Id)).ToList();
            var results = ToolbarCommandHelper.SafeExecute(command, config, orders, CommerceInstance.Current);

            command.SetDefaultCommandConfig(config);

            return AjaxForm().WithModel(results).ReloadPage();
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var order = _orderService.GetById(id);

            ViewBag.ToolbarCommands = ToolbarCommands.GetCommands(ControllerContext, order, CurrentInstance).ToList();
            ViewBag.Return = "/Commerce/Order?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
 
            return View(order);
        }

        [HttpGet]
        public ActionResult Create(int? id, int? page, int? pageSize)
        {
            Order order = null;
            if (id.HasValue && id.Value > 0)
                order = _orderService.GetById(id.Value);
            else
            {
                order = new Order();
                order.CreatedAtUtc = DateTime.Now;
            }
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath.ToLower() == "/commerce/order/selectproduct")
            {
                order = Session["TempOrder"] as Order;
            }

            var customers = _customerService.Query().OrderByDescending(x => x.Id).Paginate(page ?? 1 - 1, pageSize ?? 50).ToPagedList();
            ViewBag.Customers = customers;

            if (order.CustomerId > 0)
            {
                var customer = _customerService.GetById(order.CustomerId);
                TempData["Message"] = string.Format("Current Order: {0} {1} {2}", customer.FirstName, customer.MiddleName, customer.LastName);
            }
            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
            return View(order);
        }

        public ActionResult SelectProduct()
        {
            var order = Session["TempOrder"] as Order;
            ViewBag.SelectedCustomerId = -1;
            if (this.Request.HttpMethod == "GET")
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Form["selected_customer"]))
                {
                    order.CustomerId = Convert.ToInt32(Request.Form["selected_customer"]);
                }
                else
                {
                    TempData["Message"] = "Please select at a customer";
                    return RedirectToAction("Create", RouteValues.From(Request.QueryString));
                }
            }

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/Create?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"] + "&id=" + order.Id;
            return View(order);
        }

        public ActionResult ProductList(int page = 1, int pageSize = 50)
        {
            var query = _productService.Query();
            string search = Request.Form["search"];
            if (!string.IsNullOrEmpty(search))
            {

                query = query.Where(o => o.Name.StartsWith(search));
            }
            var products = query.OrderByDescending(x => x.Id).Paginate(page - 1, pageSize).ToPagedList();
            ViewBag.Products = products;
            ViewBag.Search = search;
            var order = Session["TempOrder"] as Order;
            return View(order);
        }

        public ActionResult FillOrderInfo()
        {
            var order = Session["TempOrder"] as Order;


            if (order.OrderItems != null)
            {
                decimal subtotal = 0.0m;
                decimal discount = 0.0m;
                decimal tax = 0.0m;
                foreach (var item in order.OrderItems)
                {
                    subtotal += item.SubTotal;
                    discount += item.Discount;
                    tax += item.TaxCost;
                }

                order.Subtotal = subtotal;
                order.Discount = discount;
                order.Tax = tax;
                order.ShippingCost = 0.0m;
                order.PaymentMethodCost = 0.0m;
                order.TotalWeight = 0.0m;
                order.Total = order.Subtotal - order.Discount + order.Tax + order.ShippingCost + order.PaymentMethodCost;
            }

            var customer = _customerService.GetById(order.CustomerId);
            order.Customer = customer;
            ViewBag.Addresses = customer.Addresses;
            ViewBag.Countries = _countryService.Query();
            //ViewBag.PaymentMethods = _paymentMethodService.GetAllPaymentMethods();
            if (order.ShippingAddress == null)
            {
                order.ShippingAddress = new OrderAddress();
                if (customer.ShippingAddress != null)
                {
                    order.ShippingAddress = OrderAddress.CreateFrom(customer.ShippingAddress);
                }
            }
            if (order.BillingAddress == null)
            {
                order.BillingAddress = new OrderAddress();
                if (customer.BillingAddress != null)
                {
                    order.BillingAddress = OrderAddress.CreateFrom(customer.BillingAddress);
                }
            }

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
            return View(order);
        }
        [HttpGet]
        public ActionResult RemoveProduct(int id)
        {
            var order = Session["TempOrder"] as Order;

            order.OrderItems.Remove(order.OrderItems.First(o => o.ProductPriceId == id));

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
            return RedirectToAction("SelectProduct", RouteValues.From(Request.QueryString));
        }
        [HttpPost]
        public ActionResult RemoveProduct()
        {
            var order = Session["TempOrder"] as Order;

            foreach (string k in Request.Form.Keys)
            {
                if (k.StartsWith("cb_"))
                {
                    var priceid = Convert.ToInt32(k.Substring(3));
                    order.OrderItems.Remove(order.OrderItems.First(o => o.ProductPriceId == priceid));
                }
            }

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
            return RedirectToAction("SelectProduct", RouteValues.From(Request.QueryString));
        }
        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {
            var order = Session["TempOrder"] as Order;

            if (order.OrderItems == null)
            {
                order.OrderItems = new List<OrderItem>();
            }

            var orderItems = FormHelper.BindToModels<OrderItem>(form);
            foreach (var item in orderItems)
            {
                if (item.Quantity > 0)
                {
                    var oldItem = order.OrderItems.FirstOrDefault(o => o.ProductPriceId == item.ProductPriceId);
                    if (oldItem == null)
                    {
                        item.SubTotal = item.Quantity * item.UnitPrice;
                        item.Total = item.SubTotal;
                        item.ProductPrice = _productService.GetProductPriceById(item.ProductPriceId);
                        order.OrderItems.Add(item);
                    }
                    else
                    {
                        oldItem.Quantity = item.Quantity;
                        oldItem.SubTotal = item.Quantity * item.UnitPrice;
                        oldItem.Total = oldItem.SubTotal;
                    }
                }
            }

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
            return RedirectToAction("SelectProduct", RouteValues.From(Request.QueryString));
        }

        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            var order = Session["TempOrder"] as Order;

            order.Coupon = form["Coupon"];
            //order.ChangeStatus((OrderStatus)Enum.Parse(typeof(OrderStatus), form["OrderStatus"]));
            order.Subtotal = string.IsNullOrEmpty(form["SubTotal"]) ? 0 : Convert.ToDecimal(form["SubTotal"]);
            order.Discount = string.IsNullOrEmpty(form["Discount"]) ? 0 : Convert.ToDecimal(form["Discount"]);
            order.Tax = string.IsNullOrEmpty(form["TotalTax"]) ? 0 : Convert.ToDecimal(form["TotalTax"]);
            order.TotalWeight = string.IsNullOrEmpty(form["TotalWeight"]) ? 0 : Convert.ToDecimal(form["TotalWeight"]);
            order.ShippingCost = string.IsNullOrEmpty(form["ShippingCost"]) ? 0 : Convert.ToDecimal(form["ShippingCost"]);
            //if (!string.IsNullOrEmpty(form["PaymentMethodId"]))
            //    order.PaymentMethodId = Convert.ToInt32(form["PaymentMethodId"]);
            order.PaymentMethodCost = string.IsNullOrEmpty(form["PaymentMethodCost"]) ? 0 : Convert.ToDecimal(form["PaymentMethodCost"]);
            order.Total = string.IsNullOrEmpty(form["Total"]) ? 0 : Convert.ToDecimal(form["Total"]);

            order.ShippingAddress = FormHelper.BindToModel<OrderAddress>(form, "ShippingAddress");
            if (order.ShippingAddress != null)
            {
                order.ShippingAddress.Id = order.ShippingAddressId.HasValue ? order.ShippingAddressId.Value : 0;
            }
            order.BillingAddress = FormHelper.BindToModel<OrderAddress>(form, "BillingAddress");
            if (order.BillingAddress != null)
            {
                order.BillingAddress.Id = order.BillingAddressId.HasValue ? order.BillingAddressId.Value : 0;
            }

            order.CustomFields = FormHelper.BindToModels<OrderCustomField>(Request.Form, "CustomFields.");

            order.Remark = form["Remark"];

            // set customer to null to avoid ef automatically insert customer as new record.
            order.Customer = null;

            _orderService.Create(order);

            return RedirectToAction("Index", RouteValues.From(Request.QueryString));
        }
    }
}