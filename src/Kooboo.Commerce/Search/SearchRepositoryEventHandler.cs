using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System.Reflection;

namespace Kooboo.Commerce.Search
{
    public class SearchRepositoryAddEventHandler : IHandles<EntityAdded>
    {
        private ISearchProvider _searchProvider;

        public SearchRepositoryAddEventHandler(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public void Handle(EntityAdded @event, EventDispatchingContext context)
        {
            var entity = @event.Entity;
            if (entity != null && AttributedTypes.SearchTypes.ContainsKey(entity.GetType().Name))
            {
                var indexer = _searchProvider.CreateIndex(entity.GetType().Name);
                var values = entity.ToDictionary();
                if (values != null && values.Count > 0)
                {
                    indexer.AddDocument(values);
                    indexer.Save();
                }
            }
        }
    }

    public class SearchRepositoryUpdateEventHandler : IHandles<EntityUpdated>
    {
        private ISearchProvider _searchProvider;

        public SearchRepositoryUpdateEventHandler(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public void Handle(EntityUpdated @event, EventDispatchingContext context)
        {
            var entity = @event.Entity;
            if (entity != null && AttributedTypes.SearchTypes.ContainsKey(entity.GetType().Name))
            {
                var indexer = _searchProvider.CreateIndex(entity.GetType().Name);
                var values = entity.ToDictionary();
                if (values != null && values.Count > 0)
                {
                    indexer.UpdateDocument(values);
                    indexer.Save();
                }
            }
        }
    }

    public class SearchRepositoryDeleteEventHandler : IHandles<EntityDeleted>
    {
        private ISearchProvider _searchProvider;

        public SearchRepositoryDeleteEventHandler(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public void Handle(EntityDeleted @event, EventDispatchingContext context)
        {
            var entity = @event.Entity;
            if (entity != null && AttributedTypes.SearchTypes.ContainsKey(entity.GetType().Name))
            {
                var indexer = _searchProvider.CreateIndex(entity.GetType().Name);
                var values = entity.ToDictionary();
                if (values != null && values.Count > 0)
                {
                    indexer.RemoveDocument(values);
                    indexer.Save();
                }
            }
        }
    }

    public static class EntityExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
                return null;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var val = prop.GetValue(obj, null);
                dic.Add(prop.Name, val);
            }

            return dic;
        }
    }
}
