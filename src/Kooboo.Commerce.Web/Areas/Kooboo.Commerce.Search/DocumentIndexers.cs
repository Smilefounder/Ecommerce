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
        static readonly ConcurrentDictionary<IndexerKey, DocumentIndexer> _indexers = new ConcurrentDictionary<IndexerKey, DocumentIndexer>();

        public static DocumentIndexer GetIndexer(string instance, CultureInfo culture, Type documentType)
        {
            return _indexers.GetOrAdd(new IndexerKey(instance, culture, documentType), key =>
            {
                lock (_indexers)
                {
                    var cultureFolderName = culture.Equals(CultureInfo.InvariantCulture) ? "Original" : culture.Name;
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Indexes\\" + cultureFolderName + "\\" + documentType.Name);
                    System.IO.Directory.CreateDirectory(path);

                    return new DocumentIndexer(key.DocumentType, Lucene.Net.Store.FSDirectory.Open(path), Analyzers.GetAnalyzer(culture));
                }
            });
        }

        class IndexerKey
        {
            public string Instance { get; private set; }

            public CultureInfo Culture { get; private set; }

            public Type DocumentType { get; private set; }

            public IndexerKey(string instance, CultureInfo culture, Type documentType)
            {
                Instance = instance;
                Culture = culture;
                DocumentType = documentType;
            }

            public override bool Equals(object obj)
            {
                var other = obj as IndexerKey;
                return other != null && other.Instance.Equals(Instance, StringComparison.OrdinalIgnoreCase) && other.Culture.Equals(Culture) && other.DocumentType.Equals(DocumentType);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = Instance.ToLowerInvariant().GetHashCode();
                    hash = hash * 397 ^ Culture.GetHashCode();
                    hash = hash * 397 ^ DocumentType.GetHashCode();
                    return hash;
                }
            }
        }
    }
}