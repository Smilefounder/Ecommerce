using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public static class DocumentSources
    {
        public static readonly Dictionary<Type, IDocumentSource> _sources = new Dictionary<Type, IDocumentSource>();

        public static IDocumentSource GetIndexSource(Type documentType)
        {
            return _sources[documentType];
        }

        static DocumentSources()
        {
            _sources.Add(typeof(Product), new ProductIndexSource());
        }
    }
}