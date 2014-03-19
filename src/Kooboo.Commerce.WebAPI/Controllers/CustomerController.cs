using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API.Customers;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CustomerController : CommerceAPIControllerBase
    {
        // GET api/customer/5
        public Customer Get(int id)
        {
            return Commerce().Customer.ById(id).FirstOrDefault();
        }

        [HttpGet]
        public Customer GetByAccount(string id)
        {
            return Commerce().Customer.ByAccountId(id).FirstOrDefault();
        }
    }
}
