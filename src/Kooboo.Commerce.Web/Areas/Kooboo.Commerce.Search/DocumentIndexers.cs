using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public static class DocumentIndexers
    {
        static readonly ConcurrentDictionary<IndexKey, DocumentIndexer> _indexers = new ConcurrentDictionary<IndexKey, DocumentIndexer>();

        public static string GetDirectory(string instance, Type documentType, CultureInfo culture, bool rebuild = false)
        {
            var cultureFolderName = culture.Equals(CultureInfo.InvariantCulture) ? "Original" : culture.Name;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Indexes\\" + cultureFolderName + "\\" + documentType.Name);
            if (rebuild)
            {
                path += "-Rebuild";
            }
            return path;
        }

        public static DocumentIndexer GetLiveIndexer(string instance, Type documentType, CultureInfo culture)
        {
            return _indexers.GetOrAdd(new IndexKey(instance, documentType, culture), key =>
            {
                lock (_indexers)
                {
                    var path = GetDirectory(instance, documentType, culture, false);
                    System.IO.Directory.CreateDirectory(path);
                    return new DocumentIndexer(key.DocumentType, Lucene.Net.Store.FSDirectory.Open(path), Analyzers.GetAnalyzer(culture));
                }
            });
        }

        public static void CloseLiveIndexer(string instance, Type documentType, CultureInfo culture)
        {
            DocumentIndexer indexer;
            if (_indexers.TryRemove(new IndexKey(instance, documentType, culture), out indexer))
            {
                indexer.Dispose();
            }
        }
    }
}