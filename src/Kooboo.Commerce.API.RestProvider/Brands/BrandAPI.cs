using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.RestProvider.Brands
{
    [Dependency(typeof(IBrandAPI), ComponentLifeStyle.Transient)]
    public class BrandAPI : RestApiAccessBase<Brand>, IBrandAPI
    {
        public IBrandQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public IBrandQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        public IBrandQuery Query()
        {
            return this;
        }
    }
}
