using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public static class IndexSources
    {
        public static readonly Dictionary<Type, IIndexSource> _sources = new Dictionary<Type, IIndexSource>();

        public static IIndexSource GetIndexSource(Type documentType)
        {
            return _sources[documentType];
        }

        static IndexSources()
        {
            _sources.Add(typeof(Product), new ProductIndexSource());
        }
    }
}