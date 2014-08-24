using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RelatedItemsReaders
    {
        static readonly Dictionary<string, List<IRelatedItemsReader>> _readersByInstances = new Dictionary<string,List<IRelatedItemsReader>>();

        public static IEnumerable<IRelatedItemsReader> GetReaders(string instance)
        {
            return _readersByInstances[instance];
        }

        public static void RemoveReaders(string instance)
        {
            _readersByInstances.Remove(instance);
        }

        public static void AddReader(string instance, IRelatedItemsReader reader)
        {
            AddReaders(instance, new[] { reader });
        }

        public static void AddReaders(string instance, IEnumerable<IRelatedItemsReader> readers)
        {
            if (!_readersByInstances.ContainsKey(instance))
            {
                _readersByInstances.Add(instance, readers.ToList());
            }
            else
            {
                _readersByInstances[instance].AddRange(readers);
            }
        }
    }
}