using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceInstanceNameResolver
    {
        string GetCurrentInstanceName();
    }

    public class CompositeCommerceInstanceNameResolver : ICommerceInstanceNameResolver
    {
        private List<ICommerceInstanceNameResolver> _resolvers;

        public CompositeCommerceInstanceNameResolver(params ICommerceInstanceNameResolver[] resolvers)
            : this(resolvers as IEnumerable<ICommerceInstanceNameResolver>)
        {
        }

        public CompositeCommerceInstanceNameResolver(IEnumerable<ICommerceInstanceNameResolver> resolvers)
        {
            Require.NotNull(resolvers, "resolvers");
            _resolvers = new List<ICommerceInstanceNameResolver>(resolvers);
        }

        public string GetCurrentInstanceName()
        {
            foreach (var resolver in _resolvers)
            {
                var commerceName = resolver.GetCurrentInstanceName();
                if (!String.IsNullOrWhiteSpace(commerceName))
                {
                    return commerceName;
                }
            }

            return null;
        }
    }

    public abstract class HttpCommerceInstanceNameResolverBase : ICommerceInstanceNameResolver
    {
        public static readonly string DefaultParamName = "commerceName";

        public string ParamName { get; set; }

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(System.Web.HttpContext.Current);

        protected HttpContextBase HttpContext
        {
            get
            {
                return HttpContextAccessor();
            }
        }

        protected HttpCommerceInstanceNameResolverBase()
            : this(DefaultParamName)
        {
        }

        protected HttpCommerceInstanceNameResolverBase(string paramName)
        {
            ParamName = paramName;
        }

        public virtual string GetCurrentInstanceName()
        {
            var httpContext = HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            return GetParamValue(ParamName, httpContext);
        }

        protected abstract string GetParamValue(string paramName, HttpContextBase httpContext);
    }

    public class QueryStringCommerceInstanceNameResolver : HttpCommerceInstanceNameResolverBase
    {
        public QueryStringCommerceInstanceNameResolver()
        {
        }

        public QueryStringCommerceInstanceNameResolver(string paramName)
            : base(paramName)
        {
        }

        protected override string GetParamValue(string paramName, HttpContextBase httpContext)
        {
            return httpContext.Request.QueryString[paramName];
        }
    }

    public class PostParamsCommerceInstanceNameResolver : HttpCommerceInstanceNameResolverBase
    {
        public PostParamsCommerceInstanceNameResolver()
        {
        }

        public PostParamsCommerceInstanceNameResolver(string paramName)
            : base(paramName)
        {
        }

        protected override string GetParamValue(string paramName, HttpContextBase httpContext)
        {
            return httpContext.Request.Form[paramName];
        }
    }

    public class HttpHeaderCommerceInstanceNameResolver : HttpCommerceInstanceNameResolverBase
    {
        public HttpHeaderCommerceInstanceNameResolver()
        {
        }

        public HttpHeaderCommerceInstanceNameResolver(string paramName)
            : base(paramName)
        {
        }

        protected override string GetParamValue(string paramName, HttpContextBase httpContext)
        {
            return httpContext.Request.Headers[paramName];
        }
    }

    public class HttpContextItemCommerceInstanceNameResolver : HttpCommerceInstanceNameResolverBase
    {
        public HttpContextItemCommerceInstanceNameResolver()
        {
        }

        public HttpContextItemCommerceInstanceNameResolver(string paramName)
            : base(paramName)
        {
        }

        protected override string GetParamValue(string paramName, HttpContextBase httpContext)
        {
            if (httpContext.Items.Contains(paramName))
                return httpContext.Items[paramName].ToString();
            return null;
        }
    }
}
