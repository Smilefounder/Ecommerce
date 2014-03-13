using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.ShoppingCarts;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CartController : CommerceAPIControllerBase
    {
        // GET api/cart/5
        public ShoppingCart Get(string sessionId, string accountId)
        {
            return Commerce().Cart.GetMyCart(sessionId, accountId);
        }

        // POST api/cart
        public bool Post([FromBody]Dictionary<string, object> form)
        {
            string sessionId = form["sessionId"].ToString();
            string accountId = form["accountId"].ToString();
            int productPriceId = Convert.ToInt32(form["productPriceId"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            return Commerce().Cart.AddToCart(sessionId, accountId, productPriceId, quantity);
        }

        // PUT api/cart/5
        public bool Put([FromBody]Dictionary<string, object> form)
        {
            string sessionId = form["sessionId"].ToString();
            string accountId = form["accountId"].ToString();
            int productPriceId = Convert.ToInt32(form["productPriceId"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            return Commerce().Cart.UpdateCart(sessionId, accountId, productPriceId, quantity);
        }

        // DELETE api/cart/5
        public bool Delete(string sessionId, string accountId)
        {
            return Commerce().Cart.ExpireShppingCart(sessionId, accountId);
        }
    }
}
