using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Orders;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Mvc.Paging;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Locations.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Payments;
using Kooboo.CMS.Common;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class OrderController : CommerceControllerBase
    {
        private readonly ICommerceDatabase _db;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ICountryService _countryService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IExtendedQueryManager _extendedQueryManager;

        public OrderController(ICommerceDatabase db, IOrderService orderService, ICustomerService customerService, IProductService productService, ICountryService countryService, IPaymentMethodService paymentMethodService,
            IExtendedQueryManager extendedQueryManager)
        {
            _db = db;
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _countryService = countryService;
            _paymentMethodService = paymentMethodService;

            _extendedQueryManager = extendedQueryManager;
        }

        [HttpGet]
        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var orders = _orderService.GetAllOrdersWithCustomer(search, page, pageSize, (o, c) =>
                {
                    o.Customer = c;
                    return o;
                });
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<Order, OrderQueryModel>();

            return View(orders);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var order = _orderService.GetById(id);
            ViewBag.Return = "/Commerce/Order?commerceName=" + Request.QueryString["commerceName"];
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

            var customers = _customerService.GetAllCustomers(null, page, pageSize);
            ViewBag.Customers = customers;

            if (order.CustomerId > 0)
            {
                var customer = _customerService.GetById(order.CustomerId);
                TempData["Message"] = string.Format("Current Order: {0} {1} {2}", customer.FirstName, customer.MiddleName, customer.LastName);
            }
            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order?commerceName=" + Request.QueryString["commerceName"];
            return View(order);
        }
        public ActionResult SelectProduct(int? pageIndex, int? pageSize)
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

            var products = _productService.GetAllProductPrices(pageIndex, pageSize);
            ViewBag.Products = products;

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/Create?commerceName=" + Request.QueryString["commerceName"] + "&id=" + order.Id;
            return View(order);
        }
        public ActionResult FillOrderInfo()
        {
            var order = Session["TempOrder"] as Order;

            if (order.ShippingAddress == null)
                order.ShippingAddress = new OrderAddress();
            if (order.BillingAddress == null)
                order.BillingAddress = new OrderAddress();

            if (order.OrderItems != null)
            {
                decimal subtotal = 0.0m;
                decimal discount = 0.0m;
                foreach (var item in order.OrderItems)
                {
                    subtotal += item.SubTotal;
                    discount += item.Discount;
                }

                order.SubTotal = subtotal;
                order.Discount = discount;
                order.TotalTax = 0.0m;
                order.ShippingCost = 0.0m;
                order.PaymentMethodCost = 0.0m;
                order.TotalWeight = 0.0m;
                order.Total = subtotal - discount + order.TotalTax + order.ShippingCost + order.PaymentMethodCost;
            }

            var customer = _customerService.GetById(order.CustomerId);
            order.Customer = customer;
            ViewBag.Addresses = customer.Addresses;
            ViewBag.Countries = _countryService.GetAllCountries();
            ViewBag.PaymentMethods = _paymentMethodService.GetAllPaymentMethods();

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?commerceName=" + Request.QueryString["commerceName"];
            return View(order);
        }
        [HttpGet]
        public ActionResult RemoveProduct(int id)
        {
            var order = Session["TempOrder"] as Order;

            order.OrderItems.Remove(order.OrderItems.First(o => o.ProductPriceId == id));

            Session["TempOrder"] = order;
            ViewBag.Return = "/Commerce/Order/SelectProduct?commerceName=" + Request.QueryString["commerceName"];
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
            ViewBag.Return = "/Commerce/Order/SelectProduct?commerceName=" + Request.QueryString["commerceName"];
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
                        item.ProductPrice = _productService.GetProductPriceById(item.ProductPriceId, true, true);
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
            ViewBag.Return = "/Commerce/Order/SelectProduct?commerceName=" + Request.QueryString["commerceName"];
            return RedirectToAction("SelectProduct", RouteValues.From(Request.QueryString));
        }
        [HttpPost]
        public ActionResult Delete(Order[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ =>
            {
                foreach (var obj in model)
                {
                    var order = _orderService.GetById(obj.Id, false);
                    _orderService.Delete(order);
                }
                data.ReloadPage = true;
            });

            return Json(data);
        }

        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            var order = Session["TempOrder"] as Order;

            order.Coupon = form["Coupon"];
            order.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), form["OrderStatus"]);
            order.PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), form["PaymentStatus"]);
            order.SubTotal = string.IsNullOrEmpty(form["SubTotal"]) ? 0 : Convert.ToDecimal(form["SubTotal"]);
            order.Discount = string.IsNullOrEmpty(form["Discount"]) ? 0 : Convert.ToDecimal(form["Discount"]);
            order.TotalTax = string.IsNullOrEmpty(form["TotalTax"]) ? 0 : Convert.ToDecimal(form["TotalTax"]);
            order.TotalWeight = string.IsNullOrEmpty(form["TotalWeight"]) ? 0 : Convert.ToDecimal(form["TotalWeight"]);
            order.ShippingCost = string.IsNullOrEmpty(form["ShippingCost"]) ? 0 : Convert.ToDecimal(form["ShippingCost"]);
            if (!string.IsNullOrEmpty(form["PaymentMethodId"]))
                order.PaymentMethodId = Convert.ToInt32(form["PaymentMethodId"]);
            order.PaymentMethodCost = string.IsNullOrEmpty(form["PaymentMethodCost"]) ? 0 : Convert.ToDecimal(form["PaymentMethodCost"]);
            order.Total = string.IsNullOrEmpty(form["Total"]) ? 0 : Convert.ToDecimal(form["Total"]);

            order.ShippingAddress = FormHelper.BindToModel<OrderAddress>(form, "ShippingAddress");
            if (order.ShippingAddress != null)
            {
                order.ShippingAddress.CustomerId = order.CustomerId;
                order.ShippingAddress.Id = order.ShippingAddressId.HasValue ? order.ShippingAddressId.Value : 0;
            }
            order.BillingAddress = FormHelper.BindToModel<OrderAddress>(form, "BillingAddress");
            if (order.BillingAddress != null)
            {
                order.BillingAddress.CustomerId = order.CustomerId;
                order.BillingAddress.Id = order.BillingAddressId.HasValue ? order.BillingAddressId.Value : 0;
            }

            order.Remark = form["Remark"];

            // set customer to null to avoid ef automatically insert customer as new record.
            order.Customer = null;

            _orderService.Save(order);

            return RedirectToAction("Index", RouteValues.From(Request.QueryString));
        }

        [HttpGet]
        public ActionResult ExtendQuery(string name, int? page, int? pageSize)
        {
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<Order, OrderQueryModel>();
            IPagedList<Order> model = null;
            var query = _extendedQueryManager.GetExtendedQuery<Order, OrderQueryModel>(name);
            if (query != null)
            {
                var paras = _extendedQueryManager.GetExtendedQueryParameters<Order, OrderQueryModel>(name);

                model = query.Query<Order>(paras, _db, page ?? 1, pageSize ?? 50, (o) => { o.Order.Customer = o.Customer; return o.Order; });

            }
            else
            {
                model = _orderService.GetAllOrdersWithCustomer(null, page, pageSize, (o, c) =>
                {
                    o.Customer = c;
                    return o;
                });
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult GetParameters(string name)
        {
            var query = _extendedQueryManager.GetExtendedQuery<Order, OrderQueryModel>(name);
            var paras = _extendedQueryManager.GetExtendedQueryParameters<Order, OrderQueryModel>(name);
            return JsonNet(new { Query = query, Parameters = paras });
        }

        [HttpPost]
        public ActionResult SaveParameters(string name, IEnumerable<ExtendedQueryParameter> parameters)
        {
            try
            {
                _extendedQueryManager.SaveExtendedQueryParameters<Order, OrderQueryModel>(name, parameters);
                return this.JsonNet(new { status = 0, message = "Parameter Saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
    }
}