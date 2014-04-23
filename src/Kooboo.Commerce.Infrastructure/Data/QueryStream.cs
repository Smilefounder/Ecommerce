using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class QueryStream<T> : IEnumerable<T>
    {
        private int _batchSize;
        private IQueryable<T> _query;

        public QueryStream(IQueryable<T> query, int batchSize)
        {
            _query = query;
            _batchSize = batchSize;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new QueryStreamEnumerator<T>(_query, _batchSize);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class QueryStreamEnumerator<T> : IEnumerator<T>
        {
            private List<T> _batchItems;
            private int _nextBatchItemIndex;
            private IQueryable<T> _query;

            private int _batchSize;
            private int _currentBatchIndex = -1;
            private bool _allLoaded;

            private T _current;

            public QueryStreamEnumerator(IQueryable<T> query, int batchSize)
            {
                _query = query;
                _batchSize = batchSize;
            }

            public T Current
            {
                get { return _current; }
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
                _current = default(T);

                if (_currentBatchIndex < 0)
                {
                    FetchNextBatch();
                }
                else if (IsEndOfBatch())
                {
                    if (_allLoaded)
                    {
                        return false;
                    }

                    FetchNextBatch();
                }

                if (!IsEndOfBatch())
                {
                    _current = _batchItems[_nextBatchItemIndex];
                    _nextBatchItemIndex++;

                    return true;
                }

                return false;
            }

            private bool IsEndOfBatch()
            {
                return _nextBatchItemIndex == _batchItems.Count;
            }

            private void FetchNextBatch()
            {
                var batch = _query.Skip((_currentBatchIndex + 1) * _batchSize)
                                  .Take(_batchSize)
                                  .ToList();

                if (batch.Count < _batchSize)
                {
                    _allLoaded = true;
                }

                _currentBatchIndex++;
                _nextBatchItemIndex = 0;
                _batchItems = batch;
            }

            public void Reset()
            {
                _batchItems = null;
                _currentBatchIndex = -1;
                _nextBatchItemIndex = 0;
            }
        }
    }
}
