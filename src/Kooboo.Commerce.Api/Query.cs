using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class Query<T>
        where T : class
    {
        private IQueryExecutor<T> _executor;

        public Dictionary<string, object> Options { get; private set; }

        public List<QueryFilter> Filters { get; private set; }

        public List<QuerySort> Sorts { get; private set; }

        public HashSet<string> Includes { get; private set; }

        public int Start { get; private set; }

        public int Limit { get; private set; }

        public Query(IQueryExecutor<T> executor)
        {
            _executor = executor;
            Options = new Dictionary<string, object>();
            Filters = new List<QueryFilter>();
            Sorts = new List<QuerySort>();
            Includes = new HashSet<string>();
            Limit = Int32.MaxValue;
        }

        public Query<T> Include(string path)
        {
            Includes.Add(path);
            return this;
        }

        public Query<T> Skip(int count)
        {
            Start = count;
            return this;
        }

        public Query<T> Take(int count)
        {
            Limit = count;
            return this;
        }

        public int Count()
        {
            return _executor.Count(this);
        }

        public T FirstOrDefault()
        {
            return _executor.FirstOrDefault(this);
        }

        public List<T> ToList()
        {
            return _executor.ToList(this);
        }
    }
}
