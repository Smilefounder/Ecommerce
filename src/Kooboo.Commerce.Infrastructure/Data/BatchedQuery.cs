using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class BatchedQuery<T> : IEnumerable<T>
    {
        private int _batchSize;
        private IQueryable<T> _query;

        public BatchedQuery(IQueryable<T> query, int batchSize)
        {
            _query = query;
            _batchSize = batchSize;
        }

        public IEnumerable<IEnumerable<T>> EnumerateBatches()
        {
            return new ByBatchEnumerable(_query, _batchSize);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var items in EnumerateBatches())
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ByBatchEnumerable : IEnumerable<IEnumerable<T>>
        {
            private IQueryable<T> _query;
            private int _batchSize;

            public ByBatchEnumerable(IQueryable<T> query, int batchSize)
            {
                _query = query;
                _batchSize = batchSize;
            }

            public IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                return new ByBatchEnumerator(_query, _batchSize);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ByBatchEnumerator : IEnumerator<IEnumerable<T>>
        {
            private IQueryable<T> _query;
            private int _batchSize;
            private int _batchIndex = -1;
            private bool _allFetched;

            public IEnumerable<T> Current { get; private set; }

            public ByBatchEnumerator(IQueryable<T> query, int batchSize)
            {
                _query = query;
                _batchSize = batchSize;
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_allFetched)
                {
                    return false;
                }

                var items = _query.Skip((_batchIndex + 1) * _batchSize)
                                  .Take(_batchSize)
                                  .ToList();

                Current = items;

                if (items.Count < _batchSize)
                {
                    _allFetched = true;
                }

                _batchIndex++;

                return items.Count > 0;
            }

            public void Reset()
            {
                _batchIndex = -1;
                Current = null;
            }
        }
    }

    public static class QueryableBatchedExtensions
    {
        public static BatchedQuery<T> Batched<T>(this IQueryable<T> query, int batchSize)
        {
            return new BatchedQuery<T>(query, batchSize);
        }
    }
}
