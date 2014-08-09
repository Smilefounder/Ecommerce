using Kooboo.Commerce.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public static class IndexSources
    {
        public static readonly Dictionary<Type, IIndexSource> _sources = new Dictionary<Type, IIndexSource>();

        public static IIndexSource GetIndexSource(Type modelType)
        {
            Require.NotNull(modelType, "modelType");
            return _sources[modelType];
        }

        static IndexSources()
        {
            _sources.Add(typeof(ProductModel), new ProductIndexSource());
        }
    }
}