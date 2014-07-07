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
    public static class SampleTextDatabase
    {
        static readonly Dictionary<EntityProperty, string> _data = new Dictionary<EntityProperty, string>();

        public static string GetText(EntityProperty property)
        {
            string value;
            if (_data.TryGetValue(property, out value))
            {
                return value;
            }

            return null;
        }

        public static void SetText(EntityProperty property, string value)
        {
            if (_data.ContainsKey(property))
            {
                _data[property] = value;
            }
            else
            {
                _data.Add(property, value);
            }
        }
    }

    public class GlobalizationHandler : IHandle<GetText>, IHandle<SetText>
    {
        public void Handle(GetText @event)
        {
            foreach (var prop in @event.Texts.Keys.ToList())
            {
                @event.Texts[prop] = SampleTextDatabase.GetText(prop);
            }
        }

        public void Handle(SetText @event)
        {
            foreach (var each in @event.Texts)
            {
                SampleTextDatabase.SetText(each.Key, each.Value);
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

            return Content(brandName);
        }

        public ActionResult SetText(string text)
        {
            var brand = new Brand
            {
                Id = 5
            };

            brand.SetText("Name", text, CultureInfo.CurrentCulture);

            return Content("OK");
        }
    }
}
