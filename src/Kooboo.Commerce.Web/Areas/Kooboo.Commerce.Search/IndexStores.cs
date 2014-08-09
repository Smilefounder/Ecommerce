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

        public static string GetDirectory(string instance, CultureInfo culture, Type modelType, bool rebuild)
        {
            var cultureFolderName = culture.Equals(CultureInfo.InvariantCulture) ? "Original" : culture.Name;
            var indexFolderName = modelType.Name;
            if (indexFolderName.EndsWith("Model"))
            {
                indexFolderName = indexFolderName.Substring(0, indexFolderName.Length - "Model".Length);
            }

            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Indexes\\" + cultureFolderName + "\\" + indexFolderName);
            if (rebuild)
            {
                path += "-Rebuild";
            }

            return path;
        }

        public static IndexStore Get<TModel>(string instance, CultureInfo culture)
            where TModel : class
        {
            return Get(instance, culture, typeof(TModel));
        }

        public static IndexStore Get(string instance, CultureInfo culture, Type modelType)
        {
            return _indexers.GetOrAdd(new IndexKey(instance, modelType, culture), key =>
            {
                // TODO: Might have threading issue. Think about multiple threads go here, and only one indexer instance is finally added to the dictionary.
                //       We have to ensure other indexers are not created or can be disposed.
                var path = GetDirectory(instance, culture, modelType, false);
                System.IO.Directory.CreateDirectory(path);
                return new IndexStore(modelType, Lucene.Net.Store.FSDirectory.Open(path), Analyzers.GetAnalyzer(culture));
            });
        }

        public static void Close(string instance, CultureInfo culture, Type modelType)
        {
            IndexStore indexer;
            if (_indexers.TryRemove(new IndexKey(instance, modelType, culture), out indexer))
            {
                indexer.Dispose();
            }
        }
    }
}