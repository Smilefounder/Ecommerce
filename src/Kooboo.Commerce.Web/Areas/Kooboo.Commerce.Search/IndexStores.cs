using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public static class IndexStores
    {
        static readonly ConcurrentDictionary<IndexKey, IndexStore> _indexers = new ConcurrentDictionary<IndexKey, IndexStore>();

        public static string GetDirectory(string instance, CultureInfo culture, Type documentType, bool rebuild)
        {
            var cultureFolderName = culture.Equals(CultureInfo.InvariantCulture) ? "Original" : culture.Name;
            var indexFolderName = documentType.Name;
            if (indexFolderName.EndsWith("Document"))
            {
                indexFolderName = indexFolderName.Substring(0, indexFolderName.Length - "Documents".Length);
            }

            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Indexes\\" + cultureFolderName + "\\" + indexFolderName);
            if (rebuild)
            {
                path += "-Rebuild";
            }

            return path;
        }

        public static IndexStore Get<TDocument>(string instance, CultureInfo culture)
            where TDocument : class
        {
            return Get(instance, culture, typeof(TDocument));
        }

        public static IndexStore Get(string instance, CultureInfo culture, Type documentType)
        {
            return _indexers.GetOrAdd(new IndexKey(instance, documentType, culture), key =>
            {
                // TODO: Might have threading issue. Think about multiple threads go here, and only one indexer instance is finally added to the dictionary.
                //       We have to ensure other indexers are not created or can be disposed.
                var path = GetDirectory(instance, culture, documentType, false);
                System.IO.Directory.CreateDirectory(path);
                return new IndexStore(documentType, Lucene.Net.Store.FSDirectory.Open(path), Analyzers.GetAnalyzer(culture));
            });
        }

        public static void Close(string instance, CultureInfo culture, Type documentType)
        {
            IndexStore indexer;
            if (_indexers.TryRemove(new IndexKey(instance, documentType, culture), out indexer))
            {
                indexer.Dispose();
            }
        }
    }
}