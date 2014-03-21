﻿using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Parsing;
using System.Text;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TestController : Controller
    {
        public CommerceInstanceContext CommerceInstanceContext { get; private set; }

        public TestController(CommerceInstanceContext context)
        {
            CommerceInstanceContext = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public void NonGenericRepo()
        {
            var repo = CommerceInstanceContext.CurrentInstance.Database.GetRepository(typeof(Brand));
            foreach (var each in repo.Query())
            {
                Response.Write(each.ToString());
            }
        }

        public void Transaction()
        {
            var db = CommerceInstanceContext.CurrentInstance.Database;

            using (var tx = db.BeginTransaction())
            {
                var brand = new Brand
                {
                    Name = "MyBrand 2"
                };

                db.GetRepository<Brand>().Insert(brand);

                tx.Commit();
            }
        }

        public string Tokenizer()
        {
            var tokenizer = new Tokenizer("CustomerName == ds:customers:\"Mouhong\" and orTotalAmount >= 3.14 or param3 < 25");
            var output = new StringBuilder();
            Token token = null;

            do
            {
                token = tokenizer.NextToken();
                if (token != null)
                {
                    output.Append(token.Kind == TokenKind.StringLiteral ? "\"" + token.Value + "\"" : token.Value).Append("<br/>");
                }

            } while (token != null);

            return output.ToString();
        }

        public string Parser()
        {
            var parser = new Parser();

            try
            {
                var exp = parser.Parse("(CustomerName == ds:customers:\"Mouhong\" or (Age >= 18 or Gender == \"Male\")) and TotalAmount < 59.98");
                return exp.ToString();
            }
            catch (ParserException ex)
            {
                return ex.Message.Replace(Environment.NewLine, "<br/>");
            }
        }
    }
}
