using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Brands.Services;

namespace Kooboo.Commerce.API.RestProvider.Brands
{
    public class RestBrandQuery : RestApiBase, IBrandQuery
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

        public Brand[] Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<Brand[]>("Pagination");
        }

        public Brand FirstOrDefault()
        {
            return Get<Brand>("First");
        }

        public Brand[] ToArray()
        {
            return Get<Brand[]>("All");
        }

        public int Count()
        {
            return Get<int>("Count");
        }
    }
}
