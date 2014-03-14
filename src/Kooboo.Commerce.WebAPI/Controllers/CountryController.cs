using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.Locations;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CountryController : CommerceAPIControllerBase
    {
        // GET api/default1
        public IEnumerable<Country> Get()
        {
            return Commerce().Country.GetAllCountries();
        }
    }
}
