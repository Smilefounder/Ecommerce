﻿using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class RemoveCartItemPlugin : SubmissionPluginBase<RemoveCartItemModel>
    {
        protected override SubmissionExecuteResult Execute(RemoveCartItemModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.RemoveItem(cartId, model.ItemId);

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.SuccessUrl, ControllerContext)
            };
        }
    }
}
