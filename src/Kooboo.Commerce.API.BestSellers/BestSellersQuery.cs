using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.LocalProvider;
using Kooboo.Commerce.API.LocalProvider.Utils;
using Kooboo.Commerce.API.Metadata;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products.ExtendedQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.API.BestSellers
{
    // TODO: Need to refactor the commerce Source for Data Sources.
    //       It shouldn't be so tied to commerce APIs.
    //       Otherwise writing a custom query will be difficult (Query method may be different like this one).
    [Dependency(typeof(IBestSellersQuery))]
    public class BestSellersQuery : IBestSellersQuery
    {
        private IMapper<Product, Kooboo.Commerce.Products.Product> _mapper;
        private HashSet<string> _includes = new HashSet<string>();

        public BestSellersQuery(IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
        {
            _mapper = mapper;
        }

        public ICommerceQuery<Product> Include(string property)
        {
            _includes.Add(property);
            return this;
        }

        public ICommerceQuery<Product> Include<TProperty>(System.Linq.Expressions.Expression<Func<Product, TProperty>> property)
        {
            foreach (var path in IncludeHelper.GetIncludePaths<Product, TProperty>(property))
            {
                _includes.Add(path);
            }
            return this;
        }

        public IListResource<Product> Pagination(int pageIndex, int pageSize)
        {
            var products = GetBestSellers(pageIndex, pageSize);
            var result = new ListResource<Product>();
            foreach (var product in products)
            {
                result.Add(_mapper.MapTo(product, _includes.ToArray()));
            }

            return result;
        }

        private IPagedList<Kooboo.Commerce.Products.Product> GetBestSellers(int pageIndex, int pageSize)
        {
            var query = new TopSoldProduct();
            var paras = new List<ExtendedQueryParameter>();
            return query.Query<Kooboo.Commerce.Products.Product>(paras, EngineContext.Current.Resolve<ICommerceDatabase>(), pageIndex, pageSize, p => p);
        }

        public Product FirstOrDefault()
        {
            return Pagination(0, 1).FirstOrDefault();
        }

        public IListResource<Product> ToArray()
        {
            return Pagination(0, Int32.MaxValue);
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public ICommerceQuery<Product> WithoutHalLinks()
        {
            return this;
        }

        public ICommerceQuery<Product> SetHalParameter(string name, object value)
        {
            return this;
        }
    }
}
