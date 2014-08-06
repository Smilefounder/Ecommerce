using Kooboo.Commerce.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public static class RebuildTasks
    {
        static readonly ConcurrentDictionary<IndexKey, RebuildTask> _tasks = new ConcurrentDictionary<IndexKey, RebuildTask>();

        public static RebuildTask GetTask(string instance, Type documentType, CultureInfo culture)
        {
            return _tasks.GetOrAdd(new IndexKey(instance, documentType, culture), key =>
            {
                return new RebuildTask(IndexSources.GetIndexSource(documentType), new RebuildTaskContext(instance, documentType, culture));
            });
        }
    }
}