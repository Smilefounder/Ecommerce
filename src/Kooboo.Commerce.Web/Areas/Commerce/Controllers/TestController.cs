using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Globalization;
using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class GlobalizationHandler : IHandle<GetTexts>
    {
        public void Handle(GetTexts @event)
        {
            foreach (var prop in @event.Texts.Keys.ToList())
            {
                @event.Texts[prop] = @event.Texts[prop] + " (Localized)";
            }
        }
    }

    public class TestController : Controller
    {
        public ActionResult GetText()
        {
            var brand = new Brand
            {
                Id = 5
            };

            var brandName = brand.GetText("Name", CultureInfo.CurrentCulture);
            var result = brand.GetTexts(new[] { "Name", "Description" }, CultureInfo.CurrentCulture);

            return Content(brandName);
        }
    }
}
